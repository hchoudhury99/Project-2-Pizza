using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaAPI.Model
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public IEnumerable<Order> Order { get; set; }
        public Payment Payment { get; set; }
    }
}
