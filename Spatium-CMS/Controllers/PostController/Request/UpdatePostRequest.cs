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
        public string? PublishDate { get; set; }
        public string? UnPublishDate { get; set; }
        public int StatusId { get; set; }
        public string AuthorId { get; set; }

        public List<TableOfContentRequest> TableOfContentRequests { get; set; } = new List<TableOfContentRequest>();
    }
}
