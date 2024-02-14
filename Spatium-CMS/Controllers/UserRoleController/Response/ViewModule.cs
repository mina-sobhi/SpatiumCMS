using Domain.ApplicationUserAggregate;

namespace Spatium_CMS.Controllers.UserRoleController.Response
{
    public class ViewModule
    {
        public string Name {  get; set; }
        public List<UserModulePermissions> UserPermissions {  get; set; }=new List<UserModulePermissions>();
    }
}
