using GladLogs.Server.Consts;
using GladLogs.Server.Contracts.Response;
using GladLogs.Server.Mapping;
using GladLogs.Server.Middleware;
using GladLogs.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace GladLogs.Server.Controllers
{
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly LogsContext _context;
        private readonly IApiKeyValidation _apiKeyValidation;
        public ChatsController( LogsContext context, IApiKeyValidation apiKeyValidation)
        {
            _context = context;
            _apiKeyValidation = apiKeyValidation;
        }


        
        [HttpGet("api/logs/chats")]
        public async Task<GetAllChatsResponse> GetAllChats()
        {
            return _context.Chats.ToList().ToGetAllChatsResponse();
        }


        [HttpPost("api/logs/chats/{chatname}")]
        public async Task<IActionResult> AddNewChat([FromRoute] string chatname)
        {
            //Auth
            var apiKey = Request.Headers[ApiKeyConsts.xApiKeyHeader];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return BadRequest();
            }

            if (!_apiKeyValidation.IsValidApiKey(apiKey!))
            {
                return Unauthorized();
            }


            _context.Chats.Add(new Chat()
            {
                ChatId = Guid.NewGuid(),
                Name = chatname,
            });


            await _context.SaveChangesAsync();
            return Ok();
        }
    }

}
