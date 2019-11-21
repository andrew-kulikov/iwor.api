﻿namespace iwor.api.DTOs
{
    public class UpdateProfileDto
    {
        public string Birthday { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
    }
}