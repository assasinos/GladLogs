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
            var start = DateTime.UtcNow.Date.Subtract(TimeSpan.FromDays(offset*7));
            var end = start.Subtract(TimeSpan.FromDays(7));
            var messages = _context.Messages
                .Where(x => (x.User.Name == username) && (x.Chat.Name == chatname))
                .Where(x => x.TimeStamp <= start && x.TimeStamp > end);

            


            //Messages or user not found
            if (messages is null || !messages.Any())
            {
                return new GetAllMessagesResponse();
            }


            return messages.ToList().ToGetAllMessagesResponse();
        }


        [HttpGet("d {chatname}/{username}")]
        public async Task<DateTime> GetOldestMessageTimeStamp([FromRoute] string chatname, [FromRoute] string username)
        {

            var time = await _context.Messages.Where(x => (x.User.Name == username) && (x.Chat.Name == chatname)).OrderByDescending(x => x.TimeStamp).FirstOrDefaultAsync();

            if (time is null || time == default)
            {
                return DateTime.UtcNow.Date;
            }

            return time.TimeStamp.Date;


        }


    }
}
