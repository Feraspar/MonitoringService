namespace MonitoringService.Api.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using MonitoringService.Core.Abstractions;
	using MonitoringService.Core.Contracts;

	[ApiController]
	[Route("api/[controller]")]
	public class DevicesController : ControllerBase
	{
		private readonly IDeviceQueryService _deviceService;
		private readonly ILogger _logger;

		public DevicesController(IDeviceQueryService deviceService, ILogger logger)
		{
			_deviceService = deviceService;
			_logger = logger;
		}

		[HttpGet]
		public async Task<ActionResult<List<DeviceListItemResponse>>> GetAll(CancellationToken ct)
		{
			_logger.LogInformation("GET deviceses requested");

			try
			{
				var result = await _deviceService.GetAllDevicesAsync(ct);
				_logger.LogInformation("GET returned {Count} devices", result.Count);

				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "GET devices failed");
				return BadRequest();
			}
		}

		[HttpGet("{id:guid}/activities")]
		public async Task<ActionResult<List<DeviceActivityItemResponse>>> GetAllActivitiesById(Guid id, CancellationToken ct)
		{
			_logger.LogInformation("GET activities for device: {DeviceId}", id);

			try
			{
				var result = await _deviceService.GetAllActivitiesByIdAsync(id, ct);
				_logger.LogInformation("GET returned {Count} activities for device {DeviceId}", result.Count, id);

				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "GET activities failed");
				return BadRequest();
			}
		}
	}
}
