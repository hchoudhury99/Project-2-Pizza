
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaAPI.Model
{
    public enum Size
    {
        ExtraSmall, Small, Medium, Large, ExtraLarge 
    }
    public enum Topping
    {
        chicken, beef, pork, spinach, onion, greenpeppers, Mushrooms,
        Tomatoes, Olives, Pepperoni, Sausage, RedPepper, Artichokes, Basil,
        None, Cheese
    }
    public enum PizzaName
    {
        chickenPizza, veggiePizza, pepperoniPizza, cheesePizza, beefPizza,
        sausagePizza, greekPizza, chicagoPizza, sicilianPizza, neapolitanPizza,
        customPizza
    }
    public enum Sauce
    {
        tomatoeSauce, garlicSauce, pestoSauce, BechamelSauce, SalsaSauce,
        BBQSauce, hummusSauce, pumpkinSauce, ranchSauce, wasabiSauce
    }
    public enum Crust
    {
        thickCrust, thinCrust, cheeseCrust, handtossedCrust, crunchyCrust,
        brooklynstyleCrust, glutenfreeCrust, softNpuffyCrust
    }
    public partial class Pizza
    {
        [Key]
        public int PizzaId { get; set; }
        public PizzaName? PizzaName{ get; set; }
        public Size Size { get; set; }
        public Crust Crust { get; set; }
        public Sauce Sauce { get; set; }
        public Topping? Topping1 { get; set; }
        public Topping? Topping2 { get; set; }
        public Topping?Topping3 { get; set; }
        public double Price { get; set; }
        public double Total()
        {
            double temp=0;
            if (this.PizzaName.ToString().Equals("customPizza"))
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
                if (this.Size == Size.ExtraSmall) { temp += 0.5; }
                else if (this.Size == Size.Small) { temp += 1.0; }
                else if (this.Size == Size.Medium) { temp += 2.0; }
                else if (this.Size == Size.Large) { temp += 3.0; }
                else { temp += 3.5; }

                return temp;
            }
            temp = 6.00;
            if (this.Size == Size.ExtraSmall) { temp += 0.5; }
            else if (this.Size == Size.Small) { temp += 1.0; }
            else if (this.Size == Size.Medium) { temp += 2.0; }
            else if (this.Size == Size.Large) { temp += 3.0; }
            else { temp += 3.5; }
            return temp;
        }
        public Order Order { get; set; }
        public int OrderId { get; set; }

    }
}
