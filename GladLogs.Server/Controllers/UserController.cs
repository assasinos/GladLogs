using GladLogs.Server.Contracts.Response;
using GladLogs.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GladLogs.Server.Helpers;


namespace GladLogs.Server.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {


        private readonly LogsContext _context;

        public UserController(LogsContext context)
        {
            _context = context;
        }

        [HttpGet("api/logs/user/{chatname}/{username}")]
        public async Task<GetAllActivityWeekResponses> GetUserActivityWeeks([FromRoute] string chatname, [FromRoute] string username)
        {
            if (chatname == string.Empty || username == string.Empty)
            {
                return new(); // Handle empty username or chatname
            }

            // Efficiently group messages by week:
            var messages = await _context.Messages.Include(x=>x.User).Include(x=>x.Chat)
                .Where(x => x.User.Name == username && x.Chat.Name == chatname).ToListAsync();
            var groupedMessages = messages.GroupBy(x => x.TimeStamp.GetWeekNumber()).ToList();

            // Filter out weeks with no activity:
            var activeWeeks = groupedMessages.Where(g => g.Any()).Select(g => g.Key).ToList();


            // If no active weeks found, return an empty list
            if (!activeWeeks.Any())
            {
                return new();
            }


            var currentWeekNumber = DateTime.UtcNow.GetWeekNumber();
            var activeWeekRangesWithOffsets = activeWeeks.Select(week =>
            {
                var offset = currentWeekNumber - week;
                return new ActivityWeekResponse() { offset = offset};
            }).ToList();


            var response = new GetAllActivityWeekResponses();
            response.weeks = activeWeekRangesWithOffsets;

            return  response;

        }


        
    }
}
