
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaAPI.Model;

namespace PizzaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class CustomersController : ControllerBase
    {
        private readonly PizzaDbContext _context;

        public CustomersController(PizzaDbContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public IEnumerable<Customer> GetCustomer()
        {
            return _context.Customer;
        }

        // GET: api/Customers/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetCustomer([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var customer = await _context.Customer.FindAsync(id);

        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(customer);
        //}

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var customer = await _context.Customer.FindAsync(id);
            var customer = _context.Customer.FirstOrDefaultAsync(x => x.UserId == Convert.ToInt32(id)).Result;
            // return customer;
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer([FromRoute] int id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.CustomerId)
            {
                return BadRequest();
            }
            //var UserId =_context. HttpContext.User.Identity.Name;
            //_context.Entry(customer).State = EntityState.Modified;
            var existingCustomer = _context.Customer.Where(s => s.CustomerId == customer.CustomerId).FirstOrDefault<Customer>();

            if (existingCustomer != null)
            {

                existingCustomer.Address = customer.Address;
                existingCustomer.PhoneNo = customer.PhoneNo;
                //existingCustomer.Orders = customer.Orders;
                //_context.Entry(customer).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<IActionResult> PostCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Customer.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok(customer);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.CustomerId == id);
        }

        private Customer SearchCustomer(int? id)
        {
            var customer = _context.Customer.FirstOrDefaultAsync(x => x.UserId == id).Result;
            return customer;
        }
    }
}