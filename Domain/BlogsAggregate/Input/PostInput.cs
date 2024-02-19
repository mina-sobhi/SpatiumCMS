

namespace Domain.BlogsAggregate.Input
{
    public class PostInput
    {
        public string Title { get;  set; }
        public string Slug { get;  set; }
        public string FeaturedImagePath { get;  set; }
        public string Content { get;  set; }
        public string MetaDescription { get;  set; }
        public int ContentLineSpacing { get;  set; }
        public string Category { get;  set; }
        public string Tag { get;  set; }
        public DateTime? PublishDate { get;  set; }
        public DateTime? UnPublishDate { get;  set; }
        public int StatusId { get;  set; }
        public string CreatedById { get;  set; }
        public string AuthorId { get;  set; }
        public int BlogId { get;  set; }
        public bool CommentsAllowed { get; set; }

        public List<TableOfContentInput> TableOfContents = new List<TableOfContentInput>();
    }
}
