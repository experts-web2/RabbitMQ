using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using RabitMqAPI.Models;
using RabitMqAPI.RabitMQ;
using RabitMqAPI.Repository;
using RabitMqAPI.Services;

namespace RabitMqAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "MongoDB CRUD API", Version = "v1" });
			});

			builder.Services.Configure<DbConfiguration>(builder.Configuration.GetSection("MongoDbConnection"));
            builder.Services.AddScoped<ICustomerService, CustomerService>();
			builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
			builder.Services.AddScoped<IRabitMQProducer, RabitMQProducer>();
			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
