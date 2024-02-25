using System.ComponentModel.DataAnnotations;

namespace Spatium_CMS.Controllers.StorageController.Request
{
    public class AddFileRequest
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public string Alt { get; set; }
        public string Dimension { get; set; }
        public int? FolderId { get; set; }
        [Required(ErrorMessage = "You Must Upload The File")]
        public IFormFile file { get; set; } 
    }

    
}
