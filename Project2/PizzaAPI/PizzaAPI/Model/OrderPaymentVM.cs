using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaAPI.Model
{
    public class OrderPaymentVM
    {
        public Order order { get; set; }
        public Payment payment { get; set; }

    }
}
