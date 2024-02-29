namespace Spatium_CMS.Controllers.CommentController.Response
{
    public class CommentResponse
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; }
        public int? ParentCommentId { get; set; }
        public int PostId { get; set; }
        public int StatusId { get; set; }
    }
}
