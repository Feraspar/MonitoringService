namespace MonitoringService.Infrastructure.Repositories
{
	using Microsoft.EntityFrameworkCore;
	using MonitoringService.Core.Abstractions;
	using MonitoringService.Core.Entities;
	using MonitoringService.Infrastructure.Persistence;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// Репозиторий для работы с БД с таблицей устройств.
	/// </summary>
	public class DeviceRepository : IDeviceRepository
	{
		#region Private Fields

		/// <summary>
		/// Контекст БД.
		/// </summary>
		private readonly MonitoringServiceDbContext _db;

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="db">Контекст БД.</param>
		public DeviceRepository(MonitoringServiceDbContext db)
		{
			_db = db;
		}

		#endregion Public Constructors

		#region Public Methods

		/// <summary>
		/// Добавляет новое устройство в таблицу БД.
		/// </summary>
		/// <param name="device">Устройство.</param>
		/// <param name="ct">Токен для отмены выполняемой операции.</param>
		public async Task AddAsync(Device device, CancellationToken ct = default)
		{
			await _db.Devices.AddAsync(device, ct);
		}

		/// <summary>
		/// Получает список всех устройств.
		/// </summary>
		/// <param name="ct">Токен для отмены выполняемой операции.</param>
		/// <returns>Список всех устройств.</returns>
		public async Task<List<Device>> GetAllAsync(CancellationToken ct = default)
		{
			var list = await _db.Devices.OrderByDescending(x => x.LastSeenAt).ToListAsync(ct);
			return list;
		}

		/// <summary>
		/// Получает устройство по его Id.
		/// </summary>
		/// <param name="id">Id устройства.</param>
		/// <param name="ct">Токен для отмены выполняемой операции.</param>
		/// <returns>Устройство.</returns>
		public async Task<Device?> GetByIdAsync(Guid id, CancellationToken ct = default)
		{
			var device = await _db.Devices.FirstOrDefaultAsync(x => x.Id == id, ct);
			return device;
		}

		#endregion Public Methods
	}
}