using Domains.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Repositories.Implementation;

namespace UnitTest
{
    public class CustomerRepositoryTests
    {
        private readonly IMongoCollection<Customer> _collection;
        private readonly DbConfiguration _settings;

        public CustomerRepositoryTests()
        {
            _settings = new DbConfiguration
            {
                ConnectionString = "mongodb+srv://Expert-web:6bgUKvovxVbGMu50@expert-web.s0xquxu.mongodb.net/?retryWrites=true&w=majority&appName=Expert-Web",
                DatabaseName = "RabbitMQDb",
                CollectionName = "Customers"
            };
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _collection = database.GetCollection<Customer>(_settings.CollectionName);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCustomers()
        {
            // Arrange
            var repository = new CustomerRepository(Options.Create(_settings));

            // Act
            var customers = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(customers);
            Assert.IsType<List<Customer>>(customers);
            Assert.True(customers.Any());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCustomerById()
        {
            // Arrange
            var repository = new CustomerRepository(Options.Create(_settings));
            var customerId = "1";
            // Act
            var customer = await repository.GetByIdAsync(customerId);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(customerId, customer.Id);
        }

        [Fact]
        public async Task CreateAsync_ShouldInsertCustomerIntoCollection()
        {
            // Arrange
            var repository = new CustomerRepository(Options.Create(_settings));
            var newCustomer = new Customer
            {
                Id = "101",
                FirstName = "John",
                LastName = "Wick",
                Email = "johnwick@gmail.com",
                Contact = "0334-3323222",
                Country = "Canada"
            };

            // Act
            var insertedCustomer = await repository.CreateAsync(newCustomer);

            // Assert
            Assert.NotNull(insertedCustomer);
            var Addcustomer = await repository.GetByIdAsync(insertedCustomer.Id);
            Assert.NotNull(Addcustomer);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReplaceCustomerInCollection()
        {
            // Arrange
            var repository = new CustomerRepository(Options.Create(_settings));
            var customerId = "1";
            var updatedCustomer = new Customer
            {
                Id = "1",
                FirstName = "Mr. John",
                LastName = "Deo",
                Email = "johndeo@gmail.com",
                Contact = "0334-3323222",
                Country = "Canada"
            };

            // Act
            await repository.UpdateAsync(customerId, updatedCustomer);

            // Assert
            var retrievedCustomer = await repository.GetByIdAsync(customerId);
            Assert.NotNull(retrievedCustomer);

        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveCustomerFromCollection()
        {
            // Arrange
            var repository = new CustomerRepository(Options.Create(_settings));
            var customerId = "101";

            // Act
            await repository.DeleteAsync(customerId);

            // Assert
            var deletedCustomer = await repository.GetByIdAsync(customerId);
            Assert.Null(deletedCustomer);

        }
    }

}


