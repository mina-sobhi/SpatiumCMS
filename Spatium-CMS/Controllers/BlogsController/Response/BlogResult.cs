namespace Spatium_CMS.Controllers.BlogsController.Response
{
    public class BlogResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FavIconPath { get; set; }
        public string OwnerFullName { get; set; }
        public string OwnerProfileImagePath { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
