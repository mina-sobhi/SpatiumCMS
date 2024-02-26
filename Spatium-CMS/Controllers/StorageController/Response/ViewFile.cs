using Spatium_CMS.Controllers.UserRoleController.Response;

namespace Spatium_CMS.Controllers.StorageController.Response
{
    public class ViewFile
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Extention { get; set; }
        public string FileSize { get; set; }
        public string UrlPath { get; set; }
        public UserResponse CreatedBy { get; set; }
        public string CreationDate { get; set; }
        public string LastUpdated { get;  set; }
    }
}
