using Microsoft.AspNetCore.Mvc;
using Moq;
using RabitMqAPI.Controllers;
using RabitMqAPI.Models;
using RabitMqAPI.RabitMQ;
using RabitMqAPI.Services;

namespace RabbitMQ.UnitTest
{
    public class CustomerControllerTests
    {
        List<Customer> expectedCustomers = new List<Customer>
            {
                new Customer { Id = "1", FirstName = "John", LastName = "Doe", Contact = "0323423" , Email = "john@gmail.com", Country = "Usa"},
                new Customer { Id = "2", FirstName = "Jane", LastName = "Smith" , Contact = "02233423" , Email = "Jane@gmail.com", Country = "Japan"}
            };

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfCustomers()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            var rabitMQProducerService = new Mock<IRabitMQProducer>();


            mockCustomerService.Setup(service => service.GetAllAsync())
                .ReturnsAsync(expectedCustomers);

            var controller = new CustomerController(mockCustomerService.Object, rabitMQProducerService.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualCustomers = Assert.IsAssignableFrom<IEnumerable<Customer>>(okResult.Value);
            Assert.Equal(expectedCustomers.Count, actualCustomers.Count());
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithValidId()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            var rabitMQProducerService = new Mock<IRabitMQProducer>();

            var expectedCustomer = new Customer { Id = "1", FirstName = "John", LastName = "Doe", Contact = "0323423", Email = "john@gmail.com", Country = "Usa" };
            mockCustomerService.Setup(service => service.GetByIdAsync("1"))
                .ReturnsAsync(expectedCustomer);

            var controller = new CustomerController(mockCustomerService.Object, rabitMQProducerService.Object);

            // Act
            var result = await controller.Get("1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualCustomer = Assert.IsAssignableFrom<Customer>(okResult.Value);
            Assert.Equal(expectedCustomer.Id, actualCustomer.Id);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WithInvalidId()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            var rabitMQProducerService = new Mock<IRabitMQProducer>();

            mockCustomerService.Setup(service => service.GetByIdAsync("invalidId"))
                .ReturnsAsync((Customer)null);

            var controller = new CustomerController(mockCustomerService.Object, rabitMQProducerService.Object);

            // Act
            var result = await controller.Get("invalidId");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsOkResult_WithValidCustomer()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            var rabitMQProducerService = new Mock<IRabitMQProducer>();
            var expectedCustomer = new Customer { Id = "3", FirstName = "John", LastName = "Doe", Contact = "0323423", Email = "john@gmail.com", Country = "Usa" };

            mockCustomerService.Setup(service => service.CreateAsync(expectedCustomer))
                .ReturnsAsync(expectedCustomer);

           

            var controller = new CustomerController(mockCustomerService.Object, rabitMQProducerService.Object);

            // Act
            var result = await controller.Create(expectedCustomer);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualCustomerId = Assert.IsAssignableFrom<Customer>(okResult.Value);
            Assert.Equal(expectedCustomer.Id, actualCustomerId.Id);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WithInvalidCustomer()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            var rabitMQProducerService = new Mock<IRabitMQProducer>();
            var invalidCustomer = new Customer(); // Invalid customer without required properties


            var controller = new CustomerController(mockCustomerService.Object, rabitMQProducerService.Object);
            controller.ModelState.AddModelError("FirstName", "First name is required"); // Simulate invalid model state

            // Act
            var result = await controller.Create(invalidCustomer);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WithValidIdAndCustomer()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            var rabitMQProducerService = new Mock<IRabitMQProducer>();
            var existingCustomer = new Customer { Id = "validId", FirstName = "John", LastName = "Doe" };
            var updatedCustomer = new Customer { Id = "validId", FirstName = "Jane", LastName = "Smith" };
            mockCustomerService.Setup(service => service.GetByIdAsync("validId"))
                .ReturnsAsync(existingCustomer);

            var controller = new CustomerController(mockCustomerService.Object, rabitMQProducerService.Object);

            // Act
            var result = await controller.Update("validId", updatedCustomer);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WithInvalidId()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            var rabitMQProducerService = new Mock<IRabitMQProducer>();
            var invalidCustomer = new Customer { Id = "invalidId", FirstName = "Invalid", LastName = "Customer" };

            var controller = new CustomerController(mockCustomerService.Object, rabitMQProducerService.Object);

            // Act
            var result = await controller.Update("invalidId", invalidCustomer);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WithValidId()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            var rabitMQProducerService = new Mock<IRabitMQProducer>();
            var existingCustomer = new Customer { Id = "1", FirstName = "John", LastName = "Doe" };
            mockCustomerService.Setup(service => service.GetByIdAsync("1"))
                .ReturnsAsync(existingCustomer);

            var controller = new CustomerController(mockCustomerService.Object, rabitMQProducerService.Object);

            // Act
            var result = await controller.Delete("1");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WithInvalidId()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            var rabitMQProducerService = new Mock<IRabitMQProducer>();
            var controller = new CustomerController(mockCustomerService.Object, rabitMQProducerService.Object);

            // Act
            var result = await controller.Delete("invalidId");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}




