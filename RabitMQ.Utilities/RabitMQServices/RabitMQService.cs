using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client.Events;

namespace Utilities.RabitMQServices
{
    public class RabitMQService : IRabitMQService
    {
        private readonly IConfiguration _configuration;
        public RabitMQService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ReceiveMessage()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_configuration["ConnectionUri"])
            };

            //Create the RabbitMQ connection using connection factory details as i mentioned above
            var connection = factory.CreateConnection();

            //Here we create channel with session and model
            var channel = connection.CreateModel();

            //declare the queue after mentioning name and a few property related to that
            channel.QueueDeclare("customer", exclusive: false);

            //Set Event object which listen message from chanel which is sent by producer
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"customer message received: {message}");
            };

            //read the message
            channel.BasicConsume(queue: "customer", autoAck: true, consumer: consumer);
        }

        public void SendMessage<T>(T message)
        {

            var factory = new ConnectionFactory()
            {
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
