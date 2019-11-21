using System;
using System.ComponentModel.DataAnnotations;

namespace iwor.api.DTOs
{
    public class RegisterDto : LoginDto
    {
        [Required] [EmailAddress] public string Email { get; set; }

        [Required]
        [Compare("Password")]
        [RegularExpression(
            "^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$",
            ErrorMessage =
                "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string PasswordConfirmation { get; set; }

        public string Birthday { get; set; }
        public string Address { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}