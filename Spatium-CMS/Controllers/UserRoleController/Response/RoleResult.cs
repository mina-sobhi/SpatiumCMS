namespace Spatium_CMS.Controllers.UserRoleController.Response
{
    public class RoleResult
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public List<string> Permissions { get; set; }
        
    }
}
