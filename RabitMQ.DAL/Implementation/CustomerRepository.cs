using Domains.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Repositories.Interface;

namespace Repositories.Implementation
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IMongoCollection<Customer> _collection;
        private readonly DbConfiguration _settings;
        public CustomerRepository(IOptions<DbConfiguration> settings)
        {
            _settings = settings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _collection = database.GetCollection<Customer>(_settings.CollectionName);
        }

        public Task<List<Customer>> GetAllAsync()
        {
            return _collection.Find(c => true).ToListAsync();
        }
        public Task<Customer> GetByIdAsync(string id)
        {
            return _collection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Customer> CreateAsync(Customer customer)
        {
            //customer.Id = ObjectId.GenerateNewId(); 
            await _collection.InsertOneAsync(customer);
            return customer;
        }
        public Task UpdateAsync(string id, Customer customer)
        {
            return _collection.ReplaceOneAsync(c => c.Id == id, customer);
        }
        public Task DeleteAsync(string id)
        {
            return _collection.DeleteOneAsync(c => c.Id == id);
        }
    }
}
