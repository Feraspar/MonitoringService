namespace MonitoringService.Core.Abstractions
{
	using MonitoringService.Core.Contracts;
	using System;
	using System.Threading.Tasks;

	/// <summary>
	/// Контракт для сервиса обработки данных со стороннего приложения.
	/// </summary>
	public interface IDeviceActivityService
	{
		#region Public Methods

		/// <summary>
		/// Принимает данные со стороннего приложения.
		/// </summary>
		/// <param name="deviceId">Id устройства.</param>
		/// <param name="name">Имя пользователя.</param>
		/// <param name="startTime">Время начала сессии.</param>
		/// <param name="endTime">Время конца сессии.</param>
		/// <param name="version">Версия приложения.</param>
		/// <param name="ct">Токен для отмены выполняемой операции.</param>
		Task<DeviceActivityResponse> IngestDataAsync(Guid deviceId, string name, DateTimeOffset startTime, DateTimeOffset endTime, string version, CancellationToken ct = default);

		#endregion Public Methods
	}
}