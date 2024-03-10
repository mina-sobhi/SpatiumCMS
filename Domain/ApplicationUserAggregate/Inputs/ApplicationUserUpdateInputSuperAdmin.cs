namespace Domain.ApplicationUserAggregate.Inputs
{
    public class ApplicationUserUpdateInputSuperAdmin
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
    }
}
