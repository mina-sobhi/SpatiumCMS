namespace Spatium_CMS.Controllers.UserRoleController.Request
{
    public class UpdateUserRoleRequest
    {
        public string Id { get; set; }  
        public string IconPath { get; set; }
        public string Description { get; set; }
        public List<int> PermissionIds { get; set; }
    }
}
