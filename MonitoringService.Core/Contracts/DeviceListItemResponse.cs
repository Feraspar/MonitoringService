namespace MonitoringService.Core.Contracts
{
	public record DeviceListItemResponse(Guid deviceId,
		string name,
		DateTimeOffset lastSeenAt,
		string version,
		int activitiesCount
		);
}