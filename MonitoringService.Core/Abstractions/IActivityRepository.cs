namespace MonitoringService.Core.Abstractions
{
	using MonitoringService.Core.Entities;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	/// Контракт для репозитория активностей устройства.
	/// </summary>
	public interface IActivityRepository
	{
		#region Public Methods

		/// <summary>
		/// Добавляет новую активность устройства в таблицу БД.
		/// </summary>
		/// <param name="deviceActivity">Активность устройства.</param>
		/// <param name="ct">Токен для отмены выполняемой операции.</param>
		Task AddAsync(DeviceActivity deviceActivity, CancellationToken ct = default);

		/// <summary>
		/// Получает все активности конкретного устройства по Id.
		/// </summary>
		/// <param name="deviceId">Id устройства.</param>
		/// <param name="ct">Токен для отмены выполняемой операции.</param>
		/// <returns>Список активностей устройства.</returns>
		Task<List<DeviceActivity>> GetAllById(Guid deviceId, CancellationToken ct = default);

		#endregion Public Methods
	}
}