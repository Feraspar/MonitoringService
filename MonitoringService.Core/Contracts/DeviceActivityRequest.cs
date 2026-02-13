namespace MonitoringService.Core.Contracts
{
	using System.ComponentModel.DataAnnotations;
	using System.Text.Json.Serialization;

	/// <summary>
	/// Контракт для создания запроса.
	/// </summary>
	/// <param name="DeviceId">Id устройства.</param>
	/// <param name="Name">Имя пользователя.</param>
	/// <param name="StartTime">Время начала сессии.</param>
	/// <param name="EndTime">Время конца сессии.</param>
	/// <param name="Version">Версия приложения.</param>
	public sealed record DeviceActivityRequest(
		[property: JsonPropertyName("_id")] 
		Guid DeviceId,

		[property: JsonPropertyName("name")]
		string Name,

		[property: JsonPropertyName("startTime")] 
		DateTimeOffset StartTime,

		[property: JsonPropertyName("endTime")] 
		DateTimeOffset EndTime,

		[property: JsonPropertyName("version")]
		string Version
	);
}