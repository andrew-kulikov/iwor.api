using System;
using System.Collections.Generic;

namespace iwor.core.Entities
{
    public class UserProfile
    {
        public string Username { get; set; }
        public string Id { get; set; }
        public ICollection<string> Roles { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime Birthday { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
    }
}