using Domain.BlogsAggregate;
using Infrastructure.Database.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Repository
{
    public class CommentRepository : RepositoryBase, ICommentRepository
    {
        public CommentRepository(SpatiumDbContent SpatiumDbContent) : base(SpatiumDbContent)
        {
        }

        public async Task CreateAsync(Comment comment)
        {
            await SpatiumDbContent.Comments.AddAsync(comment);
        }

        public async Task DeleteAsync(int id)
        {
           SpatiumDbContent.Comments.Remove(await GetByIdAsync(id));
        }

        public async Task<IEnumerable<Comment>> GetBlogsAsync()
        {
            return await SpatiumDbContent.Comments.ToListAsync();
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
           return await SpatiumDbContent.Comments.FindAsync(id);
        }

        public async Task UpdateAsync(Comment comment)
        {
            SpatiumDbContent.Comments.Update(comment);
        }
    }
}
