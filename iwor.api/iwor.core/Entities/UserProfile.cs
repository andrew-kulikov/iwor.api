namespace iwor.core.Entities
{
    public class UserProfile : PublicUserProfile
    {
        public string CardNumber { get; set; } = string.Empty;
        public double Balance { get; set; }
    }
}