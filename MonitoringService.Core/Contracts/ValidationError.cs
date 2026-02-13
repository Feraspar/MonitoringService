namespace MonitoringService.Core.Contracts
{
	/// <summary>
	/// Контракт для ошибки валидации.
	/// </summary>
	/// <param name="Field">Поле, провалившее валидацию.</param>
	/// <param name="Message">Ошибка валидации.</param>
	public record ValidationError(string Field, string Message);
}