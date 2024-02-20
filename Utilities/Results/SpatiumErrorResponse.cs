namespace Utilities.Results
{
    public class SpatiumErrorResponse
    {
        public string Message { get; set; }
        public string TimeStamp { get; } = DateTime.UtcNow.ToString();
        public string Path { get; set; }
    }
}
