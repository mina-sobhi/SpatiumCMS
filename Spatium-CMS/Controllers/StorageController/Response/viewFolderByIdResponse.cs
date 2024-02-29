namespace Spatium_CMS.Controllers.StorageController.Response
{
    public class viewFolderByIdResponse
    {
        public int Id { get; set; }

        public List<ViewFolderResponse> viewFolderResponses { get; set; } = new List<ViewFolderResponse>();
        public int NumberOfFile {  get; set; }
    }
}
