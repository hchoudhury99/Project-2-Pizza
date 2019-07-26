using PizzaAPI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWeb.catalog
{
    public class TempPizza
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public Pizza Pizza { get; set; }

    }
}
