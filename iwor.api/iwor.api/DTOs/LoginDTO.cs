﻿using System.ComponentModel.DataAnnotations;

namespace iwor.api.DTOs
{
    public class LoginDto
    {
        [Required] public string Username { get; set; }

        [Required] public string Password { get; set; }
    }
}