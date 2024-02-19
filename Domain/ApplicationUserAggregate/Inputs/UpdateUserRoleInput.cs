namespace Domain.ApplicationUserAggregate.Inputs
{
    public class UpdateUserRoleInput
    {
        public string IconPath { get; set; }
        public string Description { get; set; }
        public List<int> PermissionIds { get; set; }
    }
}