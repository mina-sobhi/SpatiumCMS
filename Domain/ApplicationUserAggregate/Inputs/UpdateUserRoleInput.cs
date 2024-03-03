namespace Domain.ApplicationUserAggregate.Inputs
{
    public class UpdateUserRoleInput
    {
        public string Description { get; set; }
        public string Color { get; set; }
        public int RoleIconId { get; set; }
        public List<int> PermissionIds { get; set; }
    }
}