namespace Infrastructure.Services.AuthinticationService
{
    public class AuthConfig
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string SecretKey { get; set; }
        public string EncryptionKey { get; set; }
        public int TokenExpireInDays { get; set; }
    }
}
