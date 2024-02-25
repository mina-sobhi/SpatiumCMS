namespace Spatium_CMS.Controllers.StorageController.Request
{
    public class DeleteBulkRequest
    {
        public List<int> FolderIds { get; set; }
        public List<int> FilesIds { get; set; }
    }
}
