using System.ComponentModel.DataAnnotations;
using Utilities.Enums;

namespace Spatium_CMS.Controllers.CommentController.Request
{
    public class CommentRequest
    {
        [Required(ErrorMessage = "Content is required")]
        [MinLength(1, ErrorMessage = "Content must be at least one character")]
        [MaxLength(2200,ErrorMessage ="Content must be less Than 2200 characters")]
        public string Content { get; set; }
        public int? ParentCommentId { get; set; }

        [Required(ErrorMessage = "PostId is required")]
        public int PostId { get; set; }
        public CommentStatusEnum? StatusId { get; set; }
    }
}
