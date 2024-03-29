﻿using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
namespace RabitMQConsole
{
	internal class Program
	{
		static void Main(string[] args)
		{
            IConfiguration configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();
			var factory = new ConnectionFactory
			{
				Uri = new Uri(configuration["ConnectionUri"])
			};

			//Create the RabbitMQ connection using connection factory details as i mentioned above
			var connection = factory.CreateConnection();

			//Here we create channel with session and model
			var channel = connection.CreateModel();

			//declare the queue after mentioning name and a few property related to that
			channel.QueueDeclare("customer", exclusive: false);

			//Set Event object which listen message from chanel which is sent by producer
			var consumer = new EventingBasicConsumer(channel);
			consumer.Received += (model, eventArgs) => {
				var body = eventArgs.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);
				Console.WriteLine($"customer message received: {message}");
			};

			//read the message
			channel.BasicConsume(queue: "customer", autoAck: true, consumer: consumer);
			Console.ReadKey();
		
		}
	}
}
