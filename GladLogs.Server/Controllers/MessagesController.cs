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
using GladLogs.Server.Helpers;

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


        

        //This returns all messages, so it's slow, and will be only used in raw display
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



        /// <summary>
        /// Get user's messages from chat in period
        /// </summary>
        /// <param name="chatname">Channel name</param>
        /// <param name="username">Username</param>
        /// <param name="offset">Number of weeks to offset</param>
        /// <returns>User's messages from a chat in week selected by offset </returns>
        [HttpGet("api/messages/{chatname}/{username}/{offset}")]
        public async Task<GetAllMessagesResponse> GetUserMessagesByChatName([FromRoute] string chatname, [FromRoute] string username, [FromRoute] uint offset)
        {

            var week = DateTime.UtcNow.GetWeekNumber() -offset;


            var messages = await _context.Messages
                .Where(x => (x.User.Name == username) && (x.Chat.Name == chatname)).ToListAsync();

            var weekMessages = messages.Where(x => x.TimeStamp.GetWeekNumber() == week).ToList();

            


            //Messages or user not found
            if (weekMessages is null || !weekMessages.Any())
            {
                return new GetAllMessagesResponse();
            }


            return weekMessages.ToList().ToGetAllMessagesResponse();
        }


    }
}
