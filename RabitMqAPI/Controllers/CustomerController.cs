using Domains.Models;
using Microsoft.AspNetCore.Mvc;
using RabitMqAPI.RabitMQ;
using Services.Interface;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IRabitMQProducer _rabitMQProducer;
        public CustomerController(ICustomerService customerService, IRabitMQProducer rabitMQProducer)
        {
            _customerService = customerService;
            _rabitMQProducer = rabitMQProducer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _customerService.GetAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var customrCreated = await _customerService.CreateAsync(customer);
            _rabitMQProducer.SendMessage(customrCreated);

            return Ok(customer);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Customer customerIn)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            await _customerService.UpdateAsync(id, customerIn);
            return Ok(customerIn);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            await _customerService.DeleteAsync(customer.Id);
            return Ok("Delete Successfully")
        }

    }
}
