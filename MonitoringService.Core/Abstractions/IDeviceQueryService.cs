namespace MonitoringService.Core.Abstractions
{
	using MonitoringService.Core.Contracts;

	public interface IDeviceQueryService
	{
		#region Public Methods

		Task<List<DeviceActivityItemResponse>> GetAllActivitiesByIdAsync(Guid deviceId, CancellationToken ct = default);

		Task<List<DeviceListItemResponse>> GetAllDevicesAsync(CancellationToken ct = default);

		#endregion Public Methods
	}
}