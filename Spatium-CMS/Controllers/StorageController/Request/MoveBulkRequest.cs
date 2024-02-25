namespace Spatium_CMS.Controllers.StorageController.Request
{
    public class MoveBulkRequest
    {
        public List<int> FolderIds { get; set; }
        public List<int> FilesIds { get; set; }
        public int? DestinationId { get; set; }
    }
}
