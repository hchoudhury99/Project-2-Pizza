using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWeb.Data
{
    public class IdentityAppContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public IdentityAppContext(DbContextOptions<IdentityAppContext> options) : base(options)
        {

        }
    }
}
