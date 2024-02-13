namespace Domain.ApplicationUserAggregate.Inputs
{
    public class UserRoleInput
    {
        public string Name { get; set; }
        public string IconPath { get;  set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string RoleOwnerId { get; set; }
        public int RoleOwnerPriority { get; set; }
        public ICollection<int> UserPermissionId { get; set; }=new List<int>();
    }
}
