namespace MonitoringService.Infrastructure.Persistence
{
	using Microsoft.EntityFrameworkCore;
	using MonitoringService.Core.Entities;

	public class MonitoringServiceDbContext : DbContext
	{
		#region Public Properties

		public DbSet<DeviceActivity> DeviceActivities => Set<DeviceActivity>();
		public DbSet<Device> Devices => Set<Device>();

		#endregion Public Properties

		#region Public Constructors

		public MonitoringServiceDbContext(DbContextOptions options) : base(options)
		{
		}

		#endregion Public Constructors

		#region Protected Methods

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Device>(entity =>
			{
				entity.ToTable("devices");

				entity.HasKey(x => x.Id);

				entity.Property(x => x.LastName).HasMaxLength(200).IsRequired();

				entity.Property(x => x.LastSeenAt).IsRequired();
				entity.HasIndex(x => x.LastSeenAt);

				entity.Property(x => x.LastVersion).HasMaxLength(50).IsRequired();
			});

			modelBuilder.Entity<DeviceActivity>(entity =>
			{
				entity.ToTable("device_activity");

				entity.HasKey(x => x.Id);
				entity.Property(x => x.Id).ValueGeneratedOnAdd();

				entity.Property(x => x.DeviceId).IsRequired();

				entity.Property(x => x.DeviceName).HasMaxLength(200).IsRequired();

				entity.Property(x => x.Version).HasMaxLength(50).IsRequired();

				entity.Property(x => x.StartTime).IsRequired();

				entity.Property(x => x.EndTime).IsRequired();

				entity.Property(x => x.CreatedAt).IsRequired();

				entity.HasOne(x => x.Device).WithMany(d => d.DeviceActivity).HasForeignKey(d => d.DeviceId).OnDelete(DeleteBehavior.Cascade);

				entity.HasIndex(x => new { x.DeviceId, x.StartTime });
			});
		}

		#endregion Protected Methods
	}
}