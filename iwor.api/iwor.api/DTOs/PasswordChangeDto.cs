namespace iwor.api.DTOs
{
    public class PasswordChangeDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirmation { get; set; }
    }
}