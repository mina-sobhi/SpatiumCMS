namespace Infrastructure.Services.AuthinticationService.Models
{
    public class TokenParameters
    {
        public TokenParameters(string token, DateTime expireDate)
        {
            Token = token;
            ExpireDate = expireDate;
        }
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
