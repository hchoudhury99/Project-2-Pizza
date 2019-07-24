using Newtonsoft.Json;
using PizzaAPI;
using PizzaAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaAPI.BLL
{
    public class CustomerBLL:ICustomer
    {
        private PizzaDbContext _context;

        public CustomerBLL(PizzaDbContext context)
        {
            _context = context;

        }
        public void AddCustomer(string JsonString)
        {
            Customer temp = JsonConvert.DeserializeObject<Customer>(JsonString);
            _context.Customer.Add(temp);
            _context.SaveChanges();
        }

        public List<String> ReturnAllCustomer()
        {
            List<String> allCustomers = new List<string>();
            foreach (var item in _context.Customer)
            {
                var temp = JsonConvert.SerializeObject(item);
                allCustomers.Add(temp);
            }
            return allCustomers;
        }



    }
}
