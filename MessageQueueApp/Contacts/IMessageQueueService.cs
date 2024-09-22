namespace MessageQueueApp.Contacts
{
    public interface IMessageQueueService
    {
        void PublishMessage(string message);
        void ConsumeMessages();
    }
}
