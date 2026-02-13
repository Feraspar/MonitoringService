namespace MonitoringService.Core.Services
{
	using Microsoft.Extensions.Logging;
	using MonitoringService.Core.Abstractions;
	using MonitoringService.Core.Contracts;
	using MonitoringService.Core.Entities;
	using MonitoringService.Core.Validation;
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// Сервис для приема и сохранения данных со стороннего приложения.
	/// </summary>
	public sealed class DeviceActivityService : IDeviceActivityService
	{
		#region Private Fields

		/// <summary>
		/// Репозитрий активностей устройства.
		/// </summary>
		private readonly IActivityRepository _activityRepository;

		/// <summary>
		/// Репозиторий устройств.
		/// </summary>
		private readonly IDeviceRepository _deviceRepository;

		/// <summary>
		/// Логгер для вывода информации.
		/// </summary>
		private readonly ILogger<DeviceActivityService> _logger;

		/// <summary>
		/// Сервис сохранения изменений в БД.
		/// </summary>
		private readonly IUnitOfWork _unitOfWork;

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		/// Конструктор класса
		/// </summary>
		/// <param name="deviceRepository">Репозиторий устройств.</param>
		/// <param name="activityRepository">Репозитрий активностей устройства.</param>
		/// <param name="unitOfWork">Сервис сохранения изменений в БД.</param>
		public DeviceActivityService(IDeviceRepository deviceRepository, IActivityRepository activityRepository, IUnitOfWork unitOfWork, ILogger<DeviceActivityService> logger)
		{
			_deviceRepository = deviceRepository;
			_activityRepository = activityRepository;
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		#endregion Public Constructors

		#region Public Methods

		/// <summary>
		/// Принимает данные со стороннего приложения.
		/// </summary>
		/// <param name="deviceId">Id устройства.</param>
		/// <param name="name">Имя пользователя.</param>
		/// <param name="startTime">Время начала сессии.</param>
		/// <param name="endTime">Время конца сессии.</param>
		/// <param name="version">Версия приложения.</param>
		/// <param name="ct">Токен для отмены выполняемой операции.</param>
		public async Task<DeviceActivityResponse> IngestDataAsync(Guid deviceId, string name, DateTimeOffset startTime, DateTimeOffset endTime, string version, CancellationToken ct = default)
		{
			var errors = Validate(deviceId, name, startTime, endTime, version);
			if (errors.Count > 0)
			{
				_logger.LogWarning("Ingest validation failed for deviceId={DeviceId}. ErrorsCount={ErrorsCount}", deviceId, errors.Count);

				throw new DomainValidationException(errors);
			}

			var now = DateTimeOffset.UtcNow;

			var device = await _deviceRepository.GetByIdAsync(deviceId, ct);

			if (device is null)
			{
				_logger.LogInformation("Device not found. Creating new device: deviceId={DeviceId}", deviceId);

				device = new Device
				{
					Id = deviceId,
					Name = name,
					LastSeenAt = now,
					LastVersion = version
				};

				await _deviceRepository.AddAsync(device, ct);
			}
			else
			{
				_logger.LogDebug("Device found. Updating snapshot fields: deviceId={DeviceId}", deviceId);

				device.Name = name;
				device.LastVersion = version;
				device.LastSeenAt = now;
			}

			var activity = new DeviceActivity
			{
				DeviceId = deviceId,
				DeviceUserName = name,
				StartTime = startTime,
				EndTime = endTime,
				Version = version,
				CreatedAt = now
			};

			await _activityRepository.AddAsync(activity, ct);

			await _unitOfWork.SaveChangesAsync(ct);

			_logger.LogInformation("Ingest saved successfully: deviceId={DeviceId}, activityId={ActivityId}", device.Id, activity.Id);

			return new DeviceActivityResponse(device.Id, activity.Id, activity.DeviceUserName, activity.StartTime, activity.EndTime, activity.Version);
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Проверяет введенные данные на валидность.
		/// </summary>
		/// <param name="deviceId">Id устройства.</param>
		/// <param name="name">Имя пользователя.</param>
		/// <param name="startTime">Время начала сессии.</param>
		/// <param name="endTime">Время конца сессии.</param>
		/// <param name="version">Версия приложения.</param>
		/// <returns>Список ошибок.</returns>
		private List<ValidationError> Validate(Guid deviceId, string name, DateTimeOffset startTime, DateTimeOffset endTime, string version)
		{
			var errors = new List<ValidationError>();

			if (deviceId == Guid.Empty)
				errors.Add(new ValidationError(nameof(deviceId), "deviceId is required"));

			if (string.IsNullOrWhiteSpace(name))
				errors.Add(new ValidationError(nameof(name), "Name is required"));

			if (name.Length > 256)
				errors.Add(new ValidationError(nameof(name), "Name length must not exceed 256 characters"));

			if (string.IsNullOrWhiteSpace(version))
				errors.Add(new ValidationError(nameof(version), "Version is required"));

			if (endTime <= startTime)
				errors.Add(new ValidationError(nameof(endTime), "EndTime must be greater than StartTime"));

			if (!SemVerValidator.IsValid(version))
				errors.Add(new ValidationError(nameof(version), "Version must be in SemVer format"));

			return errors;
		}

		#endregion Private Methods
	}
}