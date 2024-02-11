using GladLogs.Server.Contracts.Response;
using GladLogs.Server.Mapping;
using GladLogs.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace GladLogs.Server.Controllers
{
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly LogsContext _context;

        public ChatsController( LogsContext context)
        {
            _context = context;
        }


        [HttpGet("api/chats")]
        public async Task<GetAllChatsResponse> GetAllChats()
        {
            return _context.Chats.ToList().ToGetAllChatsResponse();
        }
    }

}
