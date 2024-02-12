namespace Spatium_CMS.Controllers.BLog.Request
{
    public class UpdateBlogRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FavIconPath { get; set; }
        public string OwnerId { get; set; }
    }
}
