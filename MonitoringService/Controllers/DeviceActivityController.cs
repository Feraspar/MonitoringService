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
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<DeviceActivityResponse>> Ingest([FromBody] DeviceActivityRequest request, CancellationToken ct)
		{
			_logger.LogInformation("POST device requested");

			try
			{
				var result = await _deviceActivityService.IngestDataAsync(
					request.DeviceId,
					request.Name,
					request.StartTime,
					request.EndTime,
					request.Version,
					ct);

				var response = new DeviceActivityResponse(
					result.DeviceId,
					result.ActivityId,
					result.DeviceUserName,
					result.StartTime,
					result.EndTime,
					result.Version);

				_logger.LogInformation("POST device={DeviceId} successfull", result.DeviceId);

				return Ok(response);
			}
			catch (DomainValidationException ex)
			{
				_logger.LogWarning("Validation error. Errors count={Count}", ex.Errors.Count());

				return BadRequest(new ValidationErrorResponse(ex.Errors));
			}
			catch (ArgumentException ex)
			{
				_logger.LogError(ex, "POST device failed");
				return BadRequest(new { error = ex.Message });
			}
		}
	}
}