namespace MonitoringService.Core.Abstractions
{
	using System.Threading.Tasks;

	/// <summary>
	/// Контракт для сервиса сохранения изменений в БД.
	/// </summary>
	public interface IUnitOfWork
	{
		#region Public Methods

		/// <summary>
		/// Сохраняет изменения в БД.
		/// </summary>
		/// <param name="ct">Токен для отмены выполняемой операции.</param>
		Task<int> SaveChangesAsync(CancellationToken ct = default);

		#endregion Public Methods
	}
}