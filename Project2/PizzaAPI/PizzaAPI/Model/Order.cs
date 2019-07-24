<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaAPI.Model
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Range(1,1000)]
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime Duetime { get; set; }
        public IEnumerable<Pizza> Pizza { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaAPI.Model
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Range(1,1000)]
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime Duetime { get; set; }
        public IEnumerable<Pizza> Pizza { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
>>>>>>> feat
