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
	/// Репозиторий для работы с БД с таблицей активностей устройства.
	/// </summary>
	public class ActivityRepository : IActivityRepository
	{
		#region Private Fields

		/// <summary>
		/// Контекст БД.
		/// </summary>
		private readonly MonitoringServiceDbContext _db;

		/// <summary>
		/// Логгер для вывода информации.
		/// </summary>
		private readonly ILogger<ActivityRepository> _logger;

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="db">Контекст БД.</param>
		public ActivityRepository(MonitoringServiceDbContext db, ILogger<ActivityRepository> logger)
		{
			_db = db;
			_logger = logger;
		}

		#endregion Public Constructors

		#region Public Methods

		/// <summary>
		/// Добавляет новую активность устройства в таблицу БД.
		/// </summary>
		/// <param name="deviceActivity">Активность устройства.</param>
		/// <param name="ct">Токен для отмены выполняемой операции.</param>
		public async Task AddAsync(DeviceActivity deviceActivity, CancellationToken ct = default)
		{
			await _db.DeviceActivities.AddAsync(deviceActivity, ct);
		}

		/// <summary>
		/// Получает все активности конкретного устройства по Id.
		/// </summary>
		/// <param name="deviceId">Id устройства.</param>
		/// <param name="ct">Токен для отмены выполняемой операции.</param>
		/// <returns>Список активностей устройства.</returns>
		public async Task<List<DeviceActivity>> GetAllByIdAsync(Guid deviceId, CancellationToken ct = default)
		{
			var list = await _db.DeviceActivities.Where(x => x.DeviceId == deviceId).OrderByDescending(x => x.StartTime).ToListAsync(ct);

			_logger.LogDebug("DB query result: activities count={Count} for deviceId={DeviceId}", list.Count, deviceId);

			return list;
		}

		/// <summary>
		/// Получает количество записей об активности устройства.
		/// </summary>
		/// <param name="deviceIds">Id устройств.</param>
		/// <param name="ct">Токен для отмены выполняемой операции.</param>
		public async Task<Dictionary<Guid, int>> GetCountsByDeviceIdsAsync(IEnumerable<Guid> deviceIds, CancellationToken ct = default)
		{
			var ids = deviceIds.Distinct().ToArray();
			if(ids.Length == 0)
				return new Dictionary<Guid, int>();

			var dict = await _db.DeviceActivities
				.Where(a => ids.Contains(a.DeviceId))
				.GroupBy(a => a.DeviceId)
				.Select(x => new {DeviceId = x.Key, Count = x.Count()})
				.ToDictionaryAsync(x =>  x.DeviceId, x => x.Count, ct);

			_logger.LogDebug("DB query result: counts keysCount={KeysCount}", dict.Count);

			return dict;
		}

		#endregion Public Methods
	}
}