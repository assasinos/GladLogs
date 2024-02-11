using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GladLogs.Server.Models;
using GladLogs.Server.Mapping;
using GladLogs.Server.Contracts.Response;

namespace GladLogs.Server.Controllers
{
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly LogsContext _context;

        public MessagesController(LogsContext context)
        {
            _context = context;
        }


        
        [HttpGet("api/messages/{chatname}/{username}")]
        public async Task<GetAllMessagesResponse> GetUserMessagesByChatName( [FromRoute]string chatname, [FromRoute]string username)
        {
            var messages = _context.Messages.Include(x => x.User).Include(x=>x.Chat).Where(x => (x.User.Name == username) && (x.Chat.Name == chatname));


            //Messages or user not found
            if (messages is null || !await messages.AnyAsync() )
            {
                return new GetAllMessagesResponse();
            }


            return messages.ToList().ToGetAllMessagesResponse();
        }




    }
}
