namespace Spatium_CMS.Controllers.StorageController.Request
{
    public class UpdateFileRequest
    {

        public int Id { get; set; }
     
        public string Name { get; set; }
        public string Caption { get; set; }
        public string Alt { get; set; }
        public string Dimension { get; set; }
        public IFormFile File { get; set; }
    }
}
