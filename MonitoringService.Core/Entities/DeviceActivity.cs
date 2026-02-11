namespace MonitoringService.Core.Entities
{
	using System;

	/// <summary>
	/// Сущность, которая хранит данные об активности устройства.
	/// </summary>
	public class DeviceActivity
	{
		#region Public Properties

		/// <summary>
		/// Id записи об активности.
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Id устройства.
		/// </summary>
		public Guid DeviceId { get; set; }

		/// <summary>
		/// Имя пользователя.
		/// </summary>
		public string DeviceUserName { get; set; } = string.Empty;

		/// <summary>
		/// Время начала активности.
		/// </summary>
		public DateTimeOffset StartTime { get; set; }

		/// <summary>
		/// Время конца активности.
		/// </summary>
		public DateTimeOffset EndTime { get; set; }

		/// <summary>
		/// Версия приложения.
		/// </summary>
		public string Version { get; set; } = string.Empty;

		/// <summary>
		/// Время создания записи об активности.
		/// </summary>
		public DateTimeOffset CreatedAt { get; set; }

		/// <summary>
		/// Устройство, с которого ведется сессия.
		/// </summary>
		public Device Device { get; set; } = null!;

		#endregion Public Properties
	}
}