namespace MonitoringService.Core.Abstractions
{
	using MonitoringService.Core.Contracts;

	/// <summary>
	/// Контракт для сервиса запросов к БД.
	/// </summary>
	public interface IDeviceQueryService
	{
		#region Public Methods

		/// <summary>
		/// Запрашивает все записи об устройстве.
		/// </summary>
		/// <param name="deviceId">Id устройства.</param>
		/// <param name="ct">Токен для отмены выполняемой операции.</param>
		/// <returns>Список записей об устройстве.</returns>
		Task<List<DeviceActivityItemResponse>> GetAllActivitiesByIdAsync(Guid deviceId, CancellationToken ct = default);

		/// <summary>
		/// Запрашивает все устройства.
		/// </summary>
		/// <param name="ct">Токен для отмены выполняемой операции.</param>
		/// <returns>Список устройств.</returns>
		Task<List<DeviceListItemResponse>> GetAllDevicesAsync(CancellationToken ct = default);

		#endregion Public Methods
	}
}