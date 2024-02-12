namespace Spatium_CMS.Controllers.CommentController.Request
{
    public class CommentRequest
    {
        public string Content { get; set; }
        public int? ParentCommentId { get; set; }
        public int PostId { get; set; }
        public int StatusId { get; set; }
    }
}
