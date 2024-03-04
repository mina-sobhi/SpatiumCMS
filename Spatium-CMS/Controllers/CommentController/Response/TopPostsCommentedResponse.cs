using Domain.BlogsAggregate;
using System.Text.Json.Serialization;

namespace Spatium_CMS.Controllers.CommentController.Response
{
    public class TopPostsCommentedResponse
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public string Title { get; set; }

        [JsonIgnore]
        public List<Comment> Comments { get; set; }
        public int Count {
            get
            {
                return Comments.Count;
            }
        }
    }
}
