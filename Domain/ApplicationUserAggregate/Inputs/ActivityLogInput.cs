namespace Domain.ApplicationUserAggregate.Inputs
{
    public class ActivityLogInput
    {
        public string UserId { get; private set; }
        public string Content { get; private set; }
        public string IconPath { get; private set; }
    }
}
