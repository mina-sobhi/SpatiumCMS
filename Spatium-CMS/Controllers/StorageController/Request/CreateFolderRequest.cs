namespace Spatium_CMS.Controllers.StorageController.Request
{
    public class CreateFolderRequest
    {
        //public int StorageId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
