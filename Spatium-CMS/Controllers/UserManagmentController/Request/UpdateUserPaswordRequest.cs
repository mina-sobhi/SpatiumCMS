using System.ComponentModel.DataAnnotations;

namespace Spatium_CMS.Controllers.UserManagmentController.Request
{
    public class UpdateUserPaswordRequest
    {
    
        [Required(ErrorMessage = " Currenet Password is Required ")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",ErrorMessage = "-At least 8 characters long\r\n- Contains at least one uppercase letter (A-Z)\r\n- Contains at least one lowercase letter (a-z)\r\n- Contains at least one digit (0-9)\r\n- Contains at least one special character")]
        public string CurrenetPassword { get; set; }

        [Required(ErrorMessage = " New Password is Required ")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "-At least 8 characters long\r\n- Contains at least one uppercase letter (A-Z)\r\n- Contains at least one lowercase letter (a-z)\r\n- Contains at least one digit (0-9)\r\n- Contains at least one special character")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm New Password is Required ")]
        [Compare(nameof(NewPassword), ErrorMessage ="Not Match")]
        public string ConfirmNewPassword { get; set; }



    }
}
