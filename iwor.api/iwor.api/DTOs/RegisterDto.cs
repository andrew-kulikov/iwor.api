namespace iwor.api.DTOs
{
    public class RegisterDto : LoginDto
    {
        public string Email { get; set; }
        public string PasswordConfirmation { get; set; }

        public string Birthday { get; set; }
        public string Address { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CardNumber { get; set; }
        public string ImageUrl { get; set; }
    }
}