namespace Utilities.Helpers
{
    public static class OTPGenerator
    {
        public static string GenerateOTP()
        {
            Random generator = new Random();
            return generator.Next(0, 1000000).ToString("D6");
        }
    }
}
