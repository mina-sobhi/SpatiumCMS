namespace Utilities.Results
{
    public class SpatiumResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class SpatiumResponse<T>:SpatiumResponse
    {
        public T Data { get; set; }
    }
}