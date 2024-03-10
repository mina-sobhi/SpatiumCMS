using System.ComponentModel.DataAnnotations;

namespace Spatium_CMS.Controllers.UserManagmentController.Response
{
    public class UpdateUserAsSuperAdminRequest
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string RoleId { get; set; }
        [Required]
        public string UserName  { get; set; }
        [Required]
        public string Phone { get; set; }
    }
}
