namespace Domain.ApplicationUserAggregate.Inputs
{
    public class ApplicationUserUpdateInput
    {
        public string FullName { get; set; }
        public string ProfileImagePath { get; set; }
        public string RoleId { get; set; }
        public string PhoneNumber { get; set; } 
    }
}
