using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BlogsAggregate.Input
{
    public class CommentInput
    {
        public string Content { get;  set; }
        public int? ParentCommentId { get;  set; }
        public int PostId { get;  set; }
        public string CreatedById { get; set; }
    }
}
