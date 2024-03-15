namespace Utilities.RabitMQServices
{
    public interface IRabitMQService
    {
        public void SendMessage<T>(T message);
        public void ReceiveMessage();
    }
}