namespace MonitoringService.Core.Abstractions
{
	using System;

	/// <summary>
	/// Данные от стороннего приложения.
	/// </summary>
	/// <param name="DeviceId">Id устройства.</param>
	/// <param name="ActivityId">Id записи активности.</param>
	/// <param name="DeviceUserName">Имя пользователя.</param>
	/// <param name="StartTime">Время начала сессии.</param>
	/// <param name="EndTime">Время конца сессии.</param>
	/// <param name="Version">Версия устройства.</param>
	public record DeviceActivityIngestResult(
		Guid DeviceId,
		long ActivityId,
		string DeviceUserName,
		DateTimeOffset StartTime,
		DateTimeOffset EndTime,
		string Version);
}