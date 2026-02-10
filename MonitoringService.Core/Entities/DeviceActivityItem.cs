namespace MonitoringService.Core.Entities
{
	using System;

	/// <summary>
	/// Сущность, которая хранит данные об активности устройства.
	/// </summary>
	public class DeviceActivityItem
	{
		#region Public Properties

		/// <summary>
		/// Время создания записи об активности.
		/// </summary>
		public DateTimeOffset CreatedAt { get; set; }

		/// <summary>
		/// Устройство, с которого ведется сессия.
		/// </summary>
		public DeviceItem Device { get; set; } = null!;

		/// <summary>
		/// Id Устройства.
		/// </summary>
		public Guid DeviceId { get; set; }

		/// <summary>
		/// Имя пользователя.
		/// </summary>
		public string DeviceName { get; set; } = string.Empty;

		/// <summary>
		/// Время конца активности.
		/// </summary>
		public DateTimeOffset EndTime { get; set; }

		/// <summary>
		/// Id записи об активности.
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Время начала активности.
		/// </summary>
		public DateTimeOffset StartTime { get; set; }

		/// <summary>
		/// Версия устройства.
		/// </summary>
		public string Version { get; set; } = string.Empty;

		#endregion Public Properties
	}
}