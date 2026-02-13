namespace MonitoringService.Infrastructure.Repositories
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Logging;
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

		/// <summary>
		/// Логгер для вывода информации.
		/// </summary>
		private readonly ILogger<DeviceRepository> _logger;

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="db">Контекст БД.</param>
		public DeviceRepository(MonitoringServiceDbContext db, ILogger<DeviceRepository> logger)
		{
			_db = db;
			_logger = logger;
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

			_logger.LogDebug("DB query result: devices count={Count}", list.Count);

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

			_logger.LogDebug("DB query result: device is found by id={DeviceId}", id);

			return device;
		}

		#endregion Public Methods
	}
}