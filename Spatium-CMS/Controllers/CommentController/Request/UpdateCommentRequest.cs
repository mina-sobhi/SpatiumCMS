namespace Spatium_CMS.Controllers.CommentController.Request
{
    public class UpdateCommentRequest
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public int StatusId { get; set; }
    }
}
