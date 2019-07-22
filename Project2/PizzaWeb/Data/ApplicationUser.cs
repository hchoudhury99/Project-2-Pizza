using Microsoft.AspNetCore.Identity;
using PizzaAPI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWeb.Data
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Display(Name = "Last Name")]
        public string Lastname { get; set; }

        //[Display(Name = "Customer")]
        //public Customer customer { get; set; }
    }
}
