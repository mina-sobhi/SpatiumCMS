namespace Infrastructure.Services.AuthinticationService.Models
{
    public class LoggedInUser
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Token { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public DateTime ExpireDate { get; set; }
    }
}
