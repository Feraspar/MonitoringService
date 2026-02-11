namespace MonitoringService.Core.Services
{
	using MonitoringService.Core.Abstractions;
	using MonitoringService.Core.Contracts;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	public class DeviceQueryService : IDeviceQueryService
	{
		/// <summary>
		/// Репозитрий активностей устройства.
		/// </summary>
		private readonly IActivityRepository _activityRepository;

		/// <summary>
		/// Репозиторий устройств.
		/// </summary>
		private readonly IDeviceRepository _deviceRepository;

		public DeviceQueryService(IActivityRepository activityRepository, IDeviceRepository deviceRepository)
		{
			_activityRepository = activityRepository;
			_deviceRepository = deviceRepository;
		}

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
	}
}