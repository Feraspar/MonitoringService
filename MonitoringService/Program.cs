using Microsoft.EntityFrameworkCore;
using MonitoringService.Core.Abstractions;
using MonitoringService.Infrastructure.Persistence;
using MonitoringService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

var connectionString = builder.Configuration.GetConnectionString("MonitoringServiceDb");
builder.Services.AddDbContext<MonitoringServiceDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
