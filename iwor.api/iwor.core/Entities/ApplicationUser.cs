using System;
using Microsoft.AspNetCore.Identity;

namespace iwor.core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegistrationDate { get; set; }
        public DateTime? Birthday { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
    }
}