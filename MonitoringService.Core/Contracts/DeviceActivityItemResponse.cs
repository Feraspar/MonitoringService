namespace MonitoringService.Core.Contracts
{
	/// <summary>
	/// Контракт для вывода данных пользователю о записях устройства.
	/// </summary>
	/// <param name="deviceId">Id устройства.</param>
	/// <param name="name">Имя пользователя.</param>
	/// <param name="startTime">Время начала сессии.</param>
	/// <param name="endTime">Время конца сессии.</param>
	/// <param name="version">Версия приложения.</param>
	public record DeviceActivityItemResponse(
		Guid deviceId,
		string name,
		DateTimeOffset startTime,
		DateTimeOffset endTime,
		string version
	);
}