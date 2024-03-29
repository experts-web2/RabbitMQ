﻿using Domains.Models;
using Repositories.Interface;
using Services.Interface;

namespace Services.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Task<List<Customer>> GetAllAsync()
        {
            return _customerRepository.GetAllAsync();
        }

        public Task<Customer> GetByIdAsync(string id)
        {
            return _customerRepository.GetByIdAsync(id);
        }

        public Task<Customer> CreateAsync(Customer customer)
        {
            return _customerRepository.CreateAsync(customer);
        }

        public Task UpdateAsync(string id, Customer customer)
        {
            return _customerRepository.UpdateAsync(id, customer);
        }

        public Task DeleteAsync(string id)
        {
            return _customerRepository.DeleteAsync(id);
        }
    }
}
