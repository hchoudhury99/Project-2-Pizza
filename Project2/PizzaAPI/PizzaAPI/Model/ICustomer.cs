using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaAPI.Model
{
    public interface ICustomer
    {
        void AddCustomer(string JsonString);
        List<String> ReturnAllCustomer();

    }
}
