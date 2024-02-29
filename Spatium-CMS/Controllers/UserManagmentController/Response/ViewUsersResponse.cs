using Spatium_CMS.Controllers.UserRoleController.Response;

namespace Spatium_CMS.Controllers.UserManagmentController.Response
{
    public class ViewUsersResponse
    {
        public string Id { get;  set; }
        public string FullName { get; set; }
        public string ProfileImagePath { get; set; }
        public bool IsAccountActive { get; set; }
        public string Email { get; set; }
        public string CreatedAt { get; set; }
        public string RoleName { get; set; }
    }
}
