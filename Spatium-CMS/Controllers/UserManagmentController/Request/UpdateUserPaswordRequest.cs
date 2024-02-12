using System.ComponentModel.DataAnnotations;

namespace Spatium_CMS.Controllers.UserManagmentController.Request
{
    public class UpdateUserPaswordRequest
    {
    
        [Required(ErrorMessage = " Currenet Password is Required ")]
        public string CurrenetPassword { get; set; }

        [Required(ErrorMessage = " New Password is Required ")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm New Password is Required ")]
        [Compare(nameof(NewPassword), ErrorMessage ="Not Match")]
        public string ConfirmNewPassword { get; set; }



    }
}
