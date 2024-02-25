namespace Domain.ApplicationUserAggregate.Inputs
{
    public class ActivityLogInput
    {
        public string UserId { get;  set; }
        public string Content { get;  set; }
        public int LogIconId { get;  set; }
    }
}
