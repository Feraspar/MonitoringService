namespace MonitoringService.Core.Services
{
	using Microsoft.Extensions.Logging;
	using MonitoringService.Core.Abstractions;
	using MonitoringService.Core.Contracts;
	using MonitoringService.Core.Entities;
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
		/// Сервис сохранения изменений в БД.
		/// </summary>
		private readonly IUnitOfWork _unitOfWork;

		/// <summary>
		/// Логгер для вывода информации.
		/// </summary>
		private readonly ILogger<DeviceActivityService> _logger;

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
		public async Task<DeviceActivityResponse> IngestDataAsync(Guid deviceId, string? name, DateTimeOffset startTime, DateTimeOffset endTime, string? version, CancellationToken ct = default)
		{
			if (deviceId == Guid.Empty)
			{
				_logger.LogWarning("Ingest validation failed: deviceId is empty");
				throw new ArgumentNullException("deviceId is required", nameof(deviceId));
			}

			if (endTime < startTime)
			{
				_logger.LogWarning("Ingest validation failed: endTime < startTime. deviceId={DeviceId}, startTime={StartTime}, endTime={EndTime}", deviceId, startTime, endTime);
				throw new ArgumentNullException("endTime must be >= startTime", nameof(endTime));
			}

			var now = DateTimeOffset.UtcNow;

			var incomingName = string.IsNullOrWhiteSpace(name) ? null : name.Trim();
			var incomingVersion = string.IsNullOrWhiteSpace(version) ? null : version.Trim();

			var device = await _deviceRepository.GetByIdAsync(deviceId, ct);

			if (device is null)
			{
				_logger.LogInformation("Device not found. Creating new device: deviceId={DeviceId}", deviceId);

				device = new Device
				{
					Id = deviceId,
					Name = incomingName ?? string.Empty,
					LastSeenAt = now,
					LastVersion = incomingVersion ?? string.Empty
				};

				await _deviceRepository.AddAsync(device, ct);
			}
			else
			{
				_logger.LogDebug("Device found. Updating snapshot fields: deviceId={DeviceId}", deviceId);

				if (incomingName is not null)
					device.Name = incomingName;

				if (incomingVersion is not null)
					device.LastVersion = incomingVersion;

				device.LastSeenAt = now;
			}

			var snapshotName = incomingName ?? device.Name;
			var snapshotVersion = incomingVersion ?? device.LastVersion;

			var activity = new DeviceActivity
			{
				DeviceId = deviceId,
				DeviceUserName = snapshotName ?? string.Empty,
				StartTime = startTime,
				EndTime = endTime,
				Version = snapshotVersion ?? string.Empty,
				CreatedAt = now
			};

			await _activityRepository.AddAsync(activity, ct);

			await _unitOfWork.SaveChangesAsync(ct);

			_logger.LogInformation("Ingest saved successfully: deviceId={DeviceId}, activityId={ActivityId}", device.Id, activity.Id);

			return new DeviceActivityResponse(device.Id, activity.Id, activity.DeviceUserName, activity.StartTime, activity.EndTime, activity.Version);
		}

		#endregion Public Methods
	}
}