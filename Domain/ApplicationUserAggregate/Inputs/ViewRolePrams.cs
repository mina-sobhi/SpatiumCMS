namespace Domain.ApplicationUserAggregate.Inputs
{
    public class ViewRolePrams
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool? IsActive { get; set; } = true;

    }
}
