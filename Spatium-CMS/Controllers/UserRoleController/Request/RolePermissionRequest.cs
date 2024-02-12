using System.Text.Json.Serialization;

namespace Spatium_CMS.Controllers.UserRoleController.Request
{
    public class RolePermissionRequest
    {
        public string UserRoleId { get;  set; } 
        public int UserPermissionId { get; set; }
    }
}