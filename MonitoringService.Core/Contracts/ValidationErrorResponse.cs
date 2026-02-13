namespace MonitoringService.Core.Contracts
{
	/// <summary>
	/// Контракт для ответа от сервера при ошибках валидации.
	/// </summary>
	/// <param name="Errors">Ошибки валидации.</param>
	public record ValidationErrorResponse(IReadOnlyList<ValidationError> Errors);
}