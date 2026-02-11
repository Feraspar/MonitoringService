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

		public DevicesController(IDeviceQueryService deviceService)
		{
			_deviceService = deviceService;
		}

		[HttpGet]
		public async Task<ActionResult<List<DeviceListItemResponse>>> GetAll(CancellationToken ct)
		{
			var result = await _deviceService.GetAllDevicesAsync(ct);
			return Ok(result);
		}

		[HttpGet("{id:guid}/activities")]
		public async Task<ActionResult<List<DeviceActivityItemResponse>>> GetAllActivitiesById(Guid id, CancellationToken ct)
		{
			var result = await _deviceService.GetAllActivitiesByIdAsync(id, ct);
			return Ok(result);
		}
	}
}
