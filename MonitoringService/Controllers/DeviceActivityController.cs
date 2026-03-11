namespace MonitoringService.Api.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using MonitoringService.Core.Abstractions;
	using MonitoringService.Core.Contracts;
	using MonitoringService.Core.Validation;

	[ApiController]
	[Route("api/[controller]")]
	public class DeviceActivityController : ControllerBase
	{
		private readonly IDeviceActivityService _deviceActivityService;
		private readonly ILogger<DeviceActivityController> _logger;

		public DeviceActivityController(IDeviceActivityService deviceActivityService, ILogger<DeviceActivityController> logger)
		{
			_deviceActivityService = deviceActivityService;
			_logger = logger;
		}

		[HttpPost]
		[ProducesResponseType(typeof(DeviceActivityResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<DeviceActivityResponse>> Ingest([FromBody] DeviceActivityRequest request, CancellationToken ct)
		{
			_logger.LogInformation("POST device requested");

			var result = await _deviceActivityService.IngestDataAsync(
					request.DeviceId,
					request.Name,
					request.StartTime,
					request.EndTime,
					request.Version,
					ct);

			_logger.LogInformation("POST device={DeviceId} successfull", result.DeviceId);

			return Ok(result);
		}
	}
}