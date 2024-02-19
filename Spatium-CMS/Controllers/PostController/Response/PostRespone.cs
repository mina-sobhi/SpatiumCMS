namespace Spatium_CMS.Controllers.PostController.Response
{
    public class PostRespone
    {
        public int Id { get; set; } 
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string FeaturedImagePath { get; set; }
        public string Content { get; set; }
        public string MetaDescription { get; set; }
        public int ContentLineSpacing { get; set; }
        public string Category { get; set; }
        public string Tag { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? UnPublishDate { get; set; }
        public string Status { get; set; }
        public CreatedByResponse CreatedBy { get; set; }
        public DateTime CreationDate {  get; set; }
        public string AuthorId { get; set; }

    }
}
