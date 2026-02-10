namespace MonitoringService.Core.Entities
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Сущность, которая хранит данные об устройствах.
	/// </summary>
	public class Device
	{
		#region Public Properties

		/// <summary>
		/// Id устройства.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Последнее имя пользователя.
		/// </summary>
		public string LastName { get; set; } = string.Empty;

		/// <summary>
		/// Последнее время активности.
		/// </summary>
		public DateTimeOffset LastSeenAt { get; set; }

		/// <summary>
		/// Версия на момент последнего использования.
		/// </summary>
		public string LastVersion { get; set; } = string.Empty;

		/// <summary>
		/// Список записей истории активности.
		/// </summary>
		public List<DeviceActivity> DeviceActivity { get; set; } = new();

		#endregion Public Properties
	}
}