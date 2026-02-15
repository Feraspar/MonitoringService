namespace MonitoringService.Infrastructure.Persistence
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;
	using System;
	using System.Threading.Tasks;

	/// <summary>
	/// Класс для автоматической миграции в БД.
	/// </summary>
	public static class DbMigrator
	{
		#region Public Methods

		/// <summary>
		/// Применяет миграции с задержкой между попытками подключения.
		/// </summary>
		/// <param name="services">Провайдер сервисов из DI контейнера.</param>
		/// <param name="ct">Токен для отмены выполняемой операции.</param>
		public static async Task MigrateAsync(IServiceProvider services, CancellationToken ct = default)
		{
			using var scope = services.CreateScope();

			var db = scope.ServiceProvider.GetRequiredService<MonitoringServiceDbContext>();

			const int maxAttempts = 10;
			var delay = TimeSpan.FromSeconds(2);

			for (var attempt = 1; attempt <= maxAttempts; attempt++)
			{
				try
				{
					var canConnect = await db.Database.CanConnectAsync(ct);
					if (!canConnect)
						throw new Exception("Database is not ready");

					await db.Database.MigrateAsync(ct);
					return;
				}
				catch (Exception ex)
				{
					await Task.Delay(delay, ct);
				}
			}
		}

		#endregion Public Methods
	}
}