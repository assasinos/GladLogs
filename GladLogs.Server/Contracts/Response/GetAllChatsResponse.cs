using Microsoft.AspNetCore.Mvc;

namespace GladLogs.Server.Contracts.Response
{
    public class GetAllChatsResponse
    {
        public IEnumerable<ChatResponse> Chatnames { get; set; } = Enumerable.Empty<ChatResponse>();
    }
}
