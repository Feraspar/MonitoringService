namespace MonitoringService.Core.Abstractions
{
	using MonitoringService.Core.Entities;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	/// Контракт для репозитория устройств.
	/// </summary>
	public interface IDeviceRepository
	{
		#region Public Methods

		/// <summary>
		/// Добавляет новое устройство в таблицу БД.
		/// </summary>
		/// <param name="device">Устройство.</param>
		/// <param name="ct">Токен для отмены выполняемой операции.</param>
		Task AddAsync(Device device, CancellationToken ct = default);

		/// <summary>
		/// Получает список всех устройств.
		/// </summary>
		/// <param name="ct">Токен для отмены выполняемой операции.</param>
		/// <returns>Список всех устройств.</returns>
		Task<List<Device>> GetAllAsync(CancellationToken ct = default);

		/// <summary>
		/// Получает устройство по его Id.
		/// </summary>
		/// <param name="id">Id устройства.</param>
		/// <param name="ct">Токен для отмены выполняемой операции.</param>
		/// <returns>Устройство.</returns>
		Task<Device?> GetByIdAsync(Guid id, CancellationToken ct = default);

		#endregion Public Methods
	}
}