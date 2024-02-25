using Domain.ApplicationUserAggregate;

namespace Spatium_CMS.Controllers.UserRoleController.Request
{
    public class RoleRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string? Color { get; set; }
        public int RoleIconId { get; set; }
        public ICollection<int> UserPermissionId { get; set; } = new List<int>();
    }
}
