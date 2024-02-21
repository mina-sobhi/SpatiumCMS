namespace Domain.ApplicationUserAggregate.Inputs
{
    public class UserRoleInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string RoleOwnerId { get; set; }
        public int RoleOwnerPriority { get; set; }
        public int? BlogId { get; set; }
        public int RoleIconId { get; set; }
        public string? Color { get; set; }
        public ICollection<int> UserPermissionId { get; set; } = new List<int>();
    }
}
