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
		private readonly ILogger<DevicesController> _logger;

		public DevicesController(IDeviceQueryService deviceService, ILogger<DevicesController> logger)
		{
			_deviceService = deviceService;
			_logger = logger;
		}

		[HttpGet]
		[ProducesResponseType(typeof(List<DeviceListItemResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<List<DeviceListItemResponse>>> GetAll(CancellationToken ct)
		{
			_logger.LogInformation("GET deviceses requested");

			var result = await _deviceService.GetAllDevicesAsync(ct);

			_logger.LogInformation("GET returned {Count} devices", result.Count);

			return Ok(result);
		}

		[HttpGet("{id:guid}/activities")]
		[ProducesResponseType(typeof(List<DeviceActivityItemResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<List<DeviceActivityItemResponse>>> GetAllActivitiesById(Guid id, CancellationToken ct)
		{
			_logger.LogInformation("GET activities for device={DeviceId}", id);

			var result = await _deviceService.GetAllActivitiesByIdAsync(id, ct);

			_logger.LogInformation("GET returned {Count} activities for device {DeviceId}", result.Count, id);

			return Ok(result);
		}
	}
}
