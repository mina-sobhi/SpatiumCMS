namespace Spatium_CMS.Controllers.PostController.Request
{
    public class UpdatePostRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string FeaturedImagePath { get; set; }
        public string Content { get; set; }
        public string MetaDescription { get; set; }
        public int ContentLineSpacing { get; set; }
        public string Category { get; set; }
        public string Tag { get; set; }
        public string AuthorId { get; set; }
        public List<UpdateTableOfContentRequest> TableOfContentRequests { get; set; } = new List<UpdateTableOfContentRequest>();
    }
}
