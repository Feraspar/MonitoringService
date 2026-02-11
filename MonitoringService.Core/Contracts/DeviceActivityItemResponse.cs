namespace MonitoringService.Core.Contracts
{
	public record DeviceActivityItemResponse(
		Guid deviceId,
		string name,
		DateTimeOffset startTime,
		DateTimeOffset endTime,
		string version
		);
}