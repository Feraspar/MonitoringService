namespace MonitoringService.Core.Contracts
{
	/// <summary>
	/// Контракт ответа.
	/// </summary>
	/// <param name="DeviceId">Id устройства.</param>
	/// <param name="ActivityId">Id записи активности.</param>
	/// <param name="DeviceUserName">Имя пользователя.</param>
	/// <param name="StartTime">Время начала сессии.</param>
	/// <param name="EndTime">Время конца сессии.</param>
	/// <param name="Version">Версия приложения.</param>
	public sealed record DeviceActivityResponse(
		Guid DeviceId,
		long ActivityId,
		string DeviceUserName,
		DateTimeOffset StartTime,
		DateTimeOffset EndTime,
		string Version
		);
}