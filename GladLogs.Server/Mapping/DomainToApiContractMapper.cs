using GladLogs.Server.Contracts.Response;
using GladLogs.Server.Models;

namespace GladLogs.Server.Mapping
{
    public static class DomainToApiContractMapper
    {

        public static MessageResponse ToMessageResponse(this Message message)
        {
            return new MessageResponse()
            {
                Message = message.MessageContent,
                Timestamp = message.TimeStamp,
            };
        }
        public static GetAllMessagesResponse ToGetAllMessagesResponse(this IEnumerable<Message> messages) {


            return new GetAllMessagesResponse()
            {
                messages = messages.Select(x => x.ToMessageResponse())
            };
        }



        public static ChatResponse ToChatResponse(this Chat chat)
        {
            return new ChatResponse()
            {
                Chatname = chat.Name
            };
        }

        public static GetAllChatsResponse ToGetAllChatsResponse(this IEnumerable<Chat> chats)
        {


            return new GetAllChatsResponse()
            {
                Chatnames = chats.Select(x => x.ToChatResponse())
            };
        }

    }
}
