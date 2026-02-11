namespace MonitoringService.Infrastructure.Persistence
{
	using Microsoft.EntityFrameworkCore;
	using MonitoringService.Core.Entities;

	/// <summary>
	/// Контекст для работы с базой данных.
	/// </summary>
	public class MonitoringServiceDbContext : DbContext
	{
		#region Public Properties

		/// <summary>
		/// Таблица активностей устройства.
		/// </summary>
		public DbSet<DeviceActivity> DeviceActivities => Set<DeviceActivity>();

		/// <summary>
		/// Таблица устройств.
		/// </summary>
		public DbSet<Device> Devices => Set<Device>();

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="options">Параметры конфигурации контекста базы данных.</param>
		public MonitoringServiceDbContext(DbContextOptions<MonitoringServiceDbContext> options) : base(options)
		{
		}

		#endregion Public Constructors

		#region Protected Methods

		/// <summary>
		/// Настраивает модель данных.
		/// </summary>
		/// <param name="modelBuilder">Конструктор схемы БД.</param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Device>(entity =>
			{
				entity.ToTable("devices");

				entity.HasKey(x => x.Id);

				entity.Property(x => x.Name).HasMaxLength(256).IsRequired();

				entity.Property(x => x.LastSeenAt).IsRequired();
				entity.HasIndex(x => x.LastSeenAt);

				entity.Property(x => x.LastVersion).HasMaxLength(50).IsRequired();
			});

			modelBuilder.Entity<DeviceActivity>(entity =>
			{
				entity.ToTable("device_activities");

				entity.HasKey(x => x.Id);
				entity.Property(x => x.Id).ValueGeneratedOnAdd();

				entity.Property(x => x.DeviceId).IsRequired();

				entity.Property(x => x.DeviceUserName).HasMaxLength(256).IsRequired();

				entity.Property(x => x.Version).HasMaxLength(50).IsRequired();

				entity.Property(x => x.StartTime).IsRequired();

				entity.Property(x => x.EndTime).IsRequired();

				entity.Property(x => x.CreatedAt).IsRequired();

				entity.HasOne(x => x.Device).WithMany(d => d.DeviceActivities).HasForeignKey(x => x.DeviceId).OnDelete(DeleteBehavior.Cascade);

				entity.HasIndex(x => new { x.DeviceId, x.StartTime });
			});
		}

		#endregion Protected Methods
	}
}