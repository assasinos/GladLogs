using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace GladLogs.Server.Models
{
    public class LogsContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }



        public string DbPath { get; }

        //I will use SQLITE

        public LogsContext()
        {
            if (OperatingSystem.IsLinux())
            {
                var path = Environment.GetEnvironmentVariable("HOME");
                DbPath = Path.Join(path, ".twitchLogs","logs.db");
            }
            else
            {
                var folder = Environment.SpecialFolder.LocalApplicationData;
                var path = Environment.GetFolderPath(folder);
                DbPath = Path.Join(path, "logs.db");
            }

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");


    }



    public class User
    {
        public Guid UserID { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Message> Messages { get; } = [];
    }

    public class Chat
    {
        public Guid ChatId { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Message> Messages { get; } = [];
    }
    public class Message
    {
        public Guid MessageId { get; set; }


        public Guid UserID { get; set; }
        public User User { get; set; } = null!;



        public Guid ChatId { get; set; }
        public Chat Chat { get; set; } = default!;

        public string MessageContent { get; set; } = string.Empty;

        public DateTime TimeStamp { get; set; }

    }
}
