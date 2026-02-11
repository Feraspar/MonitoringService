namespace MonitoringService.Api.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using MonitoringService.Api.Contracts;
	using MonitoringService.Core.Abstractions;

	[ApiController]
	[Route("api/[controller]")]
	public class DeviceActivityController : ControllerBase
	{
		private readonly IDeviceActivityService _deviceActivityService;

		public DeviceActivityController(IDeviceActivityService deviceActivityService)
		{
			_deviceActivityService = deviceActivityService;
		}

		[HttpPost]
		[ProducesResponseType(typeof(DeviceActivityResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<DeviceActivityResponse>> Ingest([FromBody] DeviceActivityRequest request, CancellationToken ct)
		{
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

				return Ok(response);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}
	}
}
