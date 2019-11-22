namespace iwor.core.Entities
{
    public class PublicUserProfile
    {
        public string Username { get; set; }
        public string Id { get; set; }
        public string RegistrationDate { get; set; }
        public string Birthday { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}