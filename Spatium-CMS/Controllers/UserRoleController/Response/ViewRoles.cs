using Domain.ApplicationUserAggregate;

namespace Spatium_CMS.Controllers.UserRoleController.Response
{
    public class ViewRoles
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string Color { get; set; }
        public RoleIconRespones RoleIcon { get; set; }

        public ICollection<UserResponse> ApplicationUsers { get; set; }=new List<UserResponse>();
        public int UsersCount { 
            get
            {
                return ApplicationUsers.Count();
            } 
        }

    }
}
