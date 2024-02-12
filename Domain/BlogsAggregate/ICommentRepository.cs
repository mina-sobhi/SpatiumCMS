using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BlogsAggregate
{
    public interface ICommentRepository
    {
        Task<Comment> GetByIdAsync(int id);
        Task<IEnumerable<Comment>> GetBlogsAsync();
        Task CreateAsync(Comment  comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(int id);
    }
}
