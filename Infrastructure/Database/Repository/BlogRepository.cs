using Domain.BlogsAggregate;
using Infrastructure.Database.Database;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Database.Repository
{
    public class BlogRepository : RepositoryBase, IBlogRepository
    {
        public BlogRepository(SpatiumDbContent SpatiumDbContent) : base(SpatiumDbContent)
        {
        }
        public async Task<IEnumerable<Blog>> FindAllAsync(int lastPageId , int NumberofRecord) 
        {
            var nextPage = await SpatiumDbContent.Blogs
                .OrderBy(e => e.Id)
                .Where(b => b.Id > lastPageId)
                .Take(NumberofRecord)
                .ToListAsync();
            return  nextPage;
        }
        public async Task CreateAsync(Blog blog)
        {
           await SpatiumDbContent.Blogs.AddAsync(blog);
        }

        public async Task DeleteAsync(int id)
        {
            SpatiumDbContent.Blogs.Remove(await GetByIdAsync(id));
        }

        public async Task<IEnumerable<Blog>> GetBlogsAsync()
        {
            return await SpatiumDbContent.Blogs.ToListAsync();
        }

        public async Task<Blog> GetByIdAsync(int id)
        {
           return await SpatiumDbContent.Blogs.FindAsync(id);
        }

        public async Task UpdateAsync(Blog blog)
        {
            SpatiumDbContent.Update(blog);
        }
    }
}
