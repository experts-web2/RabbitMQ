namespace RabitMqAPI.RabitMQ
{
	public interface IRabitMQProducer
	{
		public void SendMessage<T>(T message);
	}
}