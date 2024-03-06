namespace Domain.ApplicationUserAggregate.Inputs
{
    public class ViewRolePrams
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool? IsActive { get; set; } = true;

    }
}
