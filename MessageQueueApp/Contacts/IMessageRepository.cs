using MessageQueueApp.Models;

namespace MessageQueueApp.Contacts
{
    public interface IMessageRepository
    {
        Task<IEnumerable<MessageModel>> GetMessagesAsync();
        Task SaveMessageAsync(string content);
    }
}