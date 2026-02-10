namespace MonitoringService.Infrastructure.Persistence
{
	using MonitoringService.Core.Abstractions;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// Сервис сохранения изменений в БД.
	/// </summary>
	public sealed class UnitOfWork : IUnitOfWork
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
		public UnitOfWork(MonitoringServiceDbContext db)
		{
			_db = db;
		}

		#endregion Public Constructors

		#region Public Methods

		/// <summary>
		/// Сохраняет изменения в БД.
		/// </summary>
		/// <param name="ct">Токен для отмены выполняемой операции.</param>
		public Task<int> SaveChangesAsync(CancellationToken ct = default)
		{
			return _db.SaveChangesAsync(ct);
		}

		#endregion Public Methods
	}
}