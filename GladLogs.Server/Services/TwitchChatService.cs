
using GladLogs.Server.Models;
using Microsoft.Extensions.Options;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Interfaces;
using TwitchLib.Communication.Models;

namespace GladLogs.Server.Services
{
    public class TwitchChatService : BackgroundService
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly IOptions<TwitchChatOptions> _options;

        public TwitchChatService(IServiceProvider provider, IOptions<TwitchChatOptions> options)
        {
            _serviceProvider = provider;
            _options = options;
        }


        TwitchClient client;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var credentials = new ConnectionCredentials(_options.Value.twitchUsername,_options.Value.twitchOAuthToken);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 1000,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);


            var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<LogsContext>();

            var channels = context.Chats.ToList();


            client.Initialize(credentials, channels.Select(x => x.Name ).ToList() );

            client.OnMessageReceived += Client_OnMessageReceived;

            client.Connect();

            scope.Dispose();

            while (!stoppingToken.IsCancellationRequested)
            {

                scope = _serviceProvider.CreateScope();

                context = scope.ServiceProvider.GetRequiredService<LogsContext>();
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

                //Add all stored Messages
                await context.Messages.AddRangeAsync(_messages);

                await context.SaveChangesAsync();
                //Clear messages
                _messages.Clear();
                scope.Dispose();

            }
            client.Disconnect();
            return;
        }

        private List<Message> _messages = [];

        private void Client_OnMessageReceived(object? sender, OnMessageReceivedArgs e)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<LogsContext>();
            //Check if user exists
            var user = context.Users.FirstOrDefault(x => x.Name == e.ChatMessage.Username);
            if (user is null || user == default)
            {
                user = new User() { 
                    UserID = Guid.NewGuid(),
                    Name = e.ChatMessage.Username
                };

                context.Users.Add(user);
                context.SaveChanges();
            }

            var channel = context.Chats.FirstOrDefault(x=> x.Name == e.ChatMessage.Channel);


            
            if (channel is null || channel == default)
            {
                throw new Exception($"Channel {e.ChatMessage.Channel} not present in database");
            }


            var message = new Message() {
                MessageId = Guid.NewGuid(),
                MessageContent = e.ChatMessage.Message,
                TimeStamp = DateTime.UtcNow,
                UserID = user.UserID,
                ChatId = channel.ChatId
            };

            scope.Dispose();

            _messages.Add(message);
        }
    }
}
