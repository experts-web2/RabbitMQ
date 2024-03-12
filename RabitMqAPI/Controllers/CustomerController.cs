using Microsoft.AspNetCore.Mvc;
using RabitMqAPI.Models;
using RabitMqAPI.RabitMQ;
using RabitMqAPI.Services;

namespace RabitMqAPI.Controllers
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
			return Ok(await _customerService.GetAllAsync().ConfigureAwait(false));
		}
		[HttpGet("{id:length(24)}")]
		public async Task<IActionResult> Get(string id)
		{
			var customer = await _customerService.GetByIdAsync(id).ConfigureAwait(false);
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
			var customrCreated =  _customerService.CreateAsync(customer).ConfigureAwait(false);
			_rabitMQProducer.SendMessage(customrCreated);
			return Ok(customer.Id);
		}
		[HttpPut("{id:length(24)}")]
		public async Task<IActionResult> Update(string id, Customer customerIn)
		{
			var customer = await _customerService.GetByIdAsync(id).ConfigureAwait(false);
			if (customer == null)
			{
				return NotFound();
			}
			await _customerService.UpdateAsync(id, customerIn).ConfigureAwait(false);
			return NoContent();
		}
		[HttpDelete("{id:length(24)}")]
		public async Task<IActionResult> Delete(string id)
		{
			var customer = await _customerService.GetByIdAsync(id).ConfigureAwait(false);
			if (customer == null)
			{
				return NotFound();
			}
			await _customerService.DeleteAsync(customer.Id).ConfigureAwait(false);
			return NoContent();
		}
	}
}
