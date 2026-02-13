namespace MonitoringService.Core.Contracts
{
	/// <summary>
	/// Контракт для вывода данных об устройстве.
	/// </summary>
	/// <param name="deviceId">Id устройства.</param>
	/// <param name="name">Имя пользователя.</param>
	/// <param name="lastSeenAt">Дата последнего обновления.</param>
	/// <param name="version">Версия приложения.</param>
	/// <param name="activitiesCount">Количество записей об активности устройства.</param>
	public record DeviceListItemResponse(Guid deviceId,
		string name,
		DateTimeOffset lastSeenAt,
		string version,
		int activitiesCount
	);
}