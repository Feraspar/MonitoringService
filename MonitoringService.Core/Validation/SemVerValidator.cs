namespace MonitoringService.Core.Validation
{
	using System.Text.RegularExpressions;

	/// <summary>
	/// Класс для проверки формата SemVer.
	/// </summary>
	public static class SemVerValidator
	{
		#region Private Fields

		/// <summary>
		/// Регулярное выражение для валидации версии в формате SemVer.
		/// </summary>
		private static readonly Regex _semVerRegex = new(@"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-([0-9A-Za-z-]+(?:\.[0-9A-Za-z-]+)*))?(?:\+([0-9A-Za-z-]+(?:\.[0-9A-Za-z-]+)*))?$", RegexOptions.Compiled);

		#endregion Private Fields

		#region Public Methods

		/// <summary>
		/// Проверяет относится ли строка к формату SemVer.
		/// </summary>
		/// <param name="version"></param>
		public static bool IsValid(string? version)
		{
			if (string.IsNullOrWhiteSpace(version))
				return false;

			return _semVerRegex.IsMatch(version.Trim());
		}

		#endregion Public Methods
	}
}