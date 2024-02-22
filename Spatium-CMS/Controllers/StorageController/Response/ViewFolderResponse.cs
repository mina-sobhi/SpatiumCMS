namespace Spatium_CMS.Controllers.StorageController.Response
{
    public class ViewFolderResponse
    {
        public int Id { get; set; }
        public int NumberOfFolders { get; set; }
        public int NumberOfFiles { get; set; }
        public string FolderName { get; set; }
        public string CreatedBy { get; set; }
        public string ProfileImage { get; set; }

    }
}
