namespace Spatium_CMS.Controllers.StorageController.Request
{
    public class RenameFolderRequest
    {
        //public int StorageId { get; set; }
        public int? ParentId { get; set; }
        public string OldName { get; set; }
        public string NewName { get; set; }

    }
}
