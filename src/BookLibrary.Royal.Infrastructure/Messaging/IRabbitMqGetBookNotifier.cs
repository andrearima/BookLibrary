namespace BookLibrary.Royal.Infrastructure.Messaging
{
    public interface IRabbitMqGetBookNotifier
    {
        void SendMessage<T>(T message);
    }
}