using Domain.ApplicationUserAggregate;

namespace Spatium_CMS.Controllers.UserRoleController.Response
{
    public class ViewRoles
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string IconPath { get; set; }
        public string IconUrl { get; set; }
        public ICollection<UserResponse> ApplicationUsers { get; set; }
        public int UsersCount { 
            get
            {
                return ApplicationUsers.Count();
            } 
        }

    }
}
