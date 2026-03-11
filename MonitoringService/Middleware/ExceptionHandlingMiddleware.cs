namespace MonitoringService.Api.Middleware
{
	using MonitoringService.Core.Contracts;
	using MonitoringService.Core.Validation;
	using System.Net;
	using System.Text.Json;

	/// <summary>
	/// Глобальный middleware для обработки исключений.
	/// </summary>
	public sealed class ExceptionHandlingMiddleware
	{
		/// <summary>
		/// Ссылка на следующий шаг pipeline.
		/// </summary>
		private readonly RequestDelegate _next;

		/// <summary>
		/// Логгер для вывода информации.
		/// </summary>
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="next">Ссылка на следующий шаг pipeline.</param>
		/// <param name="logger">Логгер для вывода информации.</param>
		public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		/// <summary>
		/// Обрабатывает ошибки запроса.
		/// </summary>
		/// <param name="context">Объект, содержащий информацию об HTTP-запросе.</param>
		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (DomainValidationException ex)
			{
				_logger.LogWarning(ex, "Validation failed for request {Method} {Path}", context.Request.Method, context.Request.Path);

				context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
				context.Response.ContentType = "application/json";

				var response = new ValidationErrorResponse(ex.Errors);

				await context.Response.WriteAsync(JsonSerializer.Serialize(response));
			}
			catch (UnauthorizedAccessException ex)
			{
				_logger.LogWarning(ex, "Unauthorized access for request {Method} {Path}", context.Request.Method, context.Request.Path);

				context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
				context.Response.ContentType = "application/json";

				await context.Response.WriteAsync(JsonSerializer.Serialize(new
				{
					error = "Unautorized"
				}));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unhandled exception for request {Method} {Path}", context.Request.Method, context.Request.Path);

				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				context.Response.ContentType = "application/json";

				await context.Response.WriteAsync(JsonSerializer.Serialize(new
				{
					error = "Internal server error"
				}));
			}
		}
	}
}
