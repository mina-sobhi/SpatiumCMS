using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BlogsAggregate.Input
{
    public class CommentUpdateInput
    {
        public int StatusId { get; set; }
        public string Content { get; set; }

    }
}
