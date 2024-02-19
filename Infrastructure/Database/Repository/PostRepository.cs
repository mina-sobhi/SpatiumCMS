using Domain.BlogsAggregate;
using Infrastructure.Database.Database;
using Microsoft.EntityFrameworkCore;
using Domain.Base;
using Infrastructure.Extensions;
namespace Infrastructure.Database.Repository
{
    public class PostRepository : RepositoryBase, IPostRepository
    {
        public PostRepository(SpatiumDbContent SpatiumDbContent) : base(SpatiumDbContent)
        {

        }
        public async Task<IEnumerable<Post>> filterAsync(int status , string contain= "")
        {
            if (string.IsNullOrEmpty(contain))
            {
                return await SpatiumDbContent.Posts.Where(p => p.StatusId == status).ToListAsync();
            }
            return   await SpatiumDbContent.Posts.Where(p => p.StatusId == status && p.Title.Contains(contain)).ToListAsync();
        }
        public async Task<List<Post>> GetPostsAsync(GetEntitiyParams postParams,int blogId)
        {
            var query = SpatiumDbContent.Posts.Where(x=>x.BlogId==blogId).AsQueryable();

            if (!string.IsNullOrEmpty(postParams.FilterColumn) && !string.IsNullOrEmpty(postParams.FilterValue))
            {
                query = query.ApplyFilter(postParams.FilterColumn, postParams.FilterValue);
            }

            if (!string.IsNullOrEmpty(postParams.SortColumn))
            {
                query = query.ApplySort(postParams.SortColumn, postParams.IsDescending);
            }

            if (!string.IsNullOrEmpty(postParams.SearchColumn) && !string.IsNullOrEmpty(postParams.SearchValue))
            {
                query = query.ApplySearch(postParams.SearchColumn, postParams.SearchValue);
            }

            var paginatedQuery = query.Skip((postParams.Page - 1) * postParams.PageSize).Take(postParams.PageSize);

            return paginatedQuery.ToList();
        }
        public async Task<Post> GetByIdAsync(int id , int blogId)
        {
            return await SpatiumDbContent.Posts.Include( P => P.TableOfContents).FirstOrDefaultAsync(p => p.Id == id && p.BlogId==blogId);
        }
        public async Task CreateAsync(Post post)
        {
            await SpatiumDbContent.Posts.AddAsync(post);
        }
 
        public async Task UpdateAsync(Post post)
        {
            SpatiumDbContent.Posts.Update(post);
        }

        public Task<Post> GetPostByOwnerId(string userId, int postId)
        {
            return SpatiumDbContent.Posts.FirstOrDefaultAsync(x => x.CreatedById == userId && x.Id==postId);
        }
    }
}
