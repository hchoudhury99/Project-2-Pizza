using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PizzaAPI.BLL;
using PizzaAPI.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PizzaAPI.Controllers
{

    [Route("api/[controller]")]
    public class CustController : Controller
    {
        private readonly ICustomer _customerBLL;

        public CustController(ICustomer _customer)
        {
            _customerBLL = _customer;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            Customer temp = new Customer();
            //temp.CustomerId = 100;
            var str = JsonConvert.SerializeObject(temp);
            Post(str);
            return _customerBLL.ReturnAllCustomer();//.ToArray();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        //JSON string is going to passed which the method AddCustomer will deseralize
        [HttpPost]
        public void Post([FromBody]string value)
        { 
            _customerBLL.AddCustomer(value);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
