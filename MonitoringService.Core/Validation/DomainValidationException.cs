namespace MonitoringService.Core.Validation
{
	using MonitoringService.Core.Contracts;
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Исключение при ошибке валидации.
	/// </summary>
	public sealed class DomainValidationException : Exception
	{
		/// <summary>
		/// Список ошибок.
		/// </summary>
		public IReadOnlyList<ValidationError> Errors { get; }

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="errors">Список ошибок.</param>
		public DomainValidationException(IReadOnlyList<ValidationError> errors) : base("Validation failed.")
		{
			Errors = errors;
		}
	}
}