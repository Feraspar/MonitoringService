namespace MonitoringService.Core.Services
{
	using MonitoringService.Core.Abstractions;
	using MonitoringService.Core.Contracts;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// Сервис для получения устройств из БД.
	/// </summary>
	public class DeviceQueryService : IDeviceQueryService
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

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="activityRepository">Репозитрий активностей устройства.</param>
		/// <param name="deviceRepository">Репозиторий устройств.</param>
		public DeviceQueryService(IActivityRepository activityRepository, IDeviceRepository deviceRepository)
		{
			_activityRepository = activityRepository;
			_deviceRepository = deviceRepository;
		}

		#endregion Public Constructors

		#region Public Methods

		/// <summary>
		/// Запрашивает все записи об устройстве.
		/// </summary>
		/// <param name="deviceId">Id устройства.</param>
		/// <param name="ct">Токен для отмены выполняемой операции.</param>
		/// <returns>Список записей об устройстве.</returns>
		public async Task<List<DeviceActivityItemResponse>> GetAllActivitiesByIdAsync(Guid deviceId, CancellationToken ct = default)
		{
			var activities = await _activityRepository.GetAllByIdAsync(deviceId, ct);

			var response = activities.Select(a => new DeviceActivityItemResponse(
				a.DeviceId,
				a.DeviceUserName,
				a.StartTime,
				a.EndTime,
				a.Version
				)).ToList();

			return response;
		}

		/// <summary>
		/// Запрашивает все устройства.
		/// </summary>
		/// <param name="ct">Токен для отмены выполняемой операции.</param>
		/// <returns>Список устройств.</returns>
		public async Task<List<DeviceListItemResponse>> GetAllDevicesAsync(CancellationToken ct = default)
		{
			var devices = await _deviceRepository.GetAllAsync(ct);

			var ids = devices.Select(d => d.Id).ToArray();
			var counts = await _activityRepository.GetCountsByDeviceIdsAsync(ids, ct);

			var response = devices.Select(d =>
			{
				counts.TryGetValue(d.Id, out var count);

				return new DeviceListItemResponse
				(
					d.Id,
					d.Name,
					d.LastSeenAt,
					d.LastVersion,
					count
				);
			})
			.ToList();

			return response;
		}

		#endregion Public Methods
	}
}