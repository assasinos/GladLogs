namespace GladLogs.Server.Contracts.Response
{
    public class GetAllMessagesResponse
    {
        public IEnumerable<MessageResponse> messages { get; init; } = Enumerable.Empty<MessageResponse>();
    }
}
