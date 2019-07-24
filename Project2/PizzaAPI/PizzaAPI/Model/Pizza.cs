using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaAPI.Model
{
    public enum Size
    {
        Small, Medium, Large
    }
    public enum Topping
    {
        chicken, beef, pork, spinach, onion, greenpeppers
    }
    public enum PizzaName
    {
        chickenPizza, veggePizza, pepperoniePizza
    }
    public enum Sauce
    {
        tomatoeSauce, garlicSauce
    }
    public enum Crust
    {
        thickCrust, thinCrust, cheeseCrust
    }
    public partial class Pizza
    {
        [Key]
        public int PizzaId { get; set; }
        public PizzaName?  PizzaName{ get; set; }
        public Size Size { get; set; }
        public Crust Crust { get; set; }
        public Sauce Sauce { get; set; }
        public Topping? Topping1 { get; set; }
        public Topping? Topping2 { get; set; }
        public Topping? Topping3 { get; set; }
        public double Total()
        {
            double temp=0;
            if (this.PizzaName == null)
            {
                if (Topping1 != null)
                {
                    temp += 2.50;
                }
                if (Topping2 != null)
                {
                    temp += 2.50;
                }
                if (Topping3 != null)
                {
                    temp += 2.50;
                }
                if (this.Size == Size.Small) { temp += 1.0; }
                if (this.Size == Size.Medium) { temp += 2.0; }
                if (this.Size == Size.Large) { temp += 3.0; }

                return temp;
            }
            temp = 6.00;
            if (this.Size == Size.Small) { temp += 1.0; }
            if (this.Size == Size.Medium) { temp += 2.0; }
            if (this.Size == Size.Large) { temp += 3.0; }
            return temp;
        }
        public Order Order { get; set; }
        public int OrderId { get; set; }

    }
}
