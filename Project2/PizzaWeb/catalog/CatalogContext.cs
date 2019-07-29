using Microsoft.EntityFrameworkCore;
using PizzaAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWeb.catalog
{
    public class CatalogContext:DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options): base(options) { }

        //we are using Pizza.OrderID as a reference to UserID;
        public DbSet<Pizza> TempPizzas { get; set; }
        //public DbSet<Pizza> Pizzas { get; set; }
    }
}
