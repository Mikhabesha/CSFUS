using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace CSFUF.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
            public string Region { get; set; }

        public static implicit operator ApplicationUser(CSFUFDB1 v)
        {
            throw new NotImplementedException();
        }
    }

 
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
   
}