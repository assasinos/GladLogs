namespace GladLogs.Server.Contracts.Response
{
    public class GetAllActivityWeekResponses
    {
        public IEnumerable<ActivityWeekResponse> weeks { get; set; } = Enumerable.Empty<ActivityWeekResponse>();

    }
}
