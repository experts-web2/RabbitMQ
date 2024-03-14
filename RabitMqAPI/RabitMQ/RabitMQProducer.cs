
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;

namespace RabitMqAPI.RabitMQ
{
	public class RabitMQProducer : IRabitMQProducer
    {
        private readonly IConfiguration _configuration;
        public RabitMQProducer(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendMessage<T>(T message)
		{

            var factory = new ConnectionFactory()
            {
               // Uri = new Uri("amqps://tiazptpq:wz-cem92aDRKMq-hr4HmcH2xEskGb3hV@fish.rmq.cloudamqp.com/tiazptpq")
                Uri = new Uri(_configuration["ConnectionUri"])
            };

            //Create the RabbitMQ connection using connection factory details as i mentioned abov
            var connection = factory.CreateConnection();

            //Here we create channel with session and model
            var channel = connection.CreateModel();

            //declare the queue after mentioning name and a few property related to that
            channel.QueueDeclare("customer", exclusive: false);

			//Serialize the message
			var json = JsonConvert.SerializeObject(message);
			var body = Encoding.UTF8.GetBytes(json);

            //put the data on to the customer queue
            channel.BasicPublish(exchange: "", routingKey: "customer", body: body);
		}
	}
}
