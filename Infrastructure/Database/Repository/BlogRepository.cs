using Domain.Base;
using Domain.BlogsAggregate;
using Infrastructure.Database.Database;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Database.Repository
{
    public class BlogRepository : RepositoryBase, IBlogRepository
    {
        public BlogRepository(SpatiumDbContent SpatiumDbContent) : base(SpatiumDbContent)
        {
        }
        #region Blog    
                public async Task<IEnumerable<Blog>> FindAllAsync(int lastPageId, int NumberofRecord)
                {
                    var nextPage = await SpatiumDbContent.Blogs
                        .OrderBy(e => e.Id)
                        .Where(b => b.Id > lastPageId)
                        .Take(NumberofRecord)
                        .ToListAsync();
                    return nextPage;
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

        #endregion

        #region Comment
        public async Task CreateCommentAsync(Comment comment)
        {
            await SpatiumDbContent.Comments.AddAsync(comment);
        }

        public async Task DeleteCommentAsync(int id)
        {
            SpatiumDbContent.Comments.Remove(await GetCommentByIdAsync(id));
        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync()
        {
            return await SpatiumDbContent.Comments.ToListAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(int id)
        {
            return await SpatiumDbContent.Comments.FindAsync(id);
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            SpatiumDbContent.Comments.Update(comment);
        }
        #endregion

        #region Post
        public async Task<IEnumerable<Post>> filterAsync(int status, string contain = "")
        {
            if (string.IsNullOrEmpty(contain))
            {
                return await SpatiumDbContent.Posts.Where(p => p.StatusId == status).ToListAsync();
            }
            return await SpatiumDbContent.Posts.Where(p => p.StatusId == status && p.Title.Contains(contain)).ToListAsync();
        }
        public async Task<List<Post>> GetPostsAsync(GetEntitiyParams postParams, int blogId)
        {
            var query = SpatiumDbContent.Posts.Where(x => x.BlogId == blogId).AsQueryable();

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
        public async Task<Post> GetPostByIdAsync(int id, int blogId)
        {
            return await SpatiumDbContent.Posts.Include(P => P.TableOfContents).FirstOrDefaultAsync(p => p.Id == id && p.BlogId == blogId);
        }
        public async Task<Post> PostSnippetPreview(int postId)
        {
            return await SpatiumDbContent.Posts.FindAsync(postId);
        }
        public async Task CreatePostAsync(Post post)
        {
            await SpatiumDbContent.Posts.AddAsync(post);
        }

        public async Task UpdatePostAsync(Post post)
        {
            SpatiumDbContent.Posts.Update(post);
        }

        public Task<Post> GetPostByOwnerId(string userId, int postId)
        {
            return SpatiumDbContent.Posts.FirstOrDefaultAsync(x => x.CreatedById == userId && x.Id == postId);
        }

        public async Task<Post> GetPostByExpression(Expression<Func<Post, bool>> expression)
        {
            return await SpatiumDbContent.Posts.Where(expression).FirstOrDefaultAsync();
        }
        #endregion

        #region TableOfContent
        public async Task CreateTableOfContentAsync(TableOfContent tableOfContent)
        {
            await SpatiumDbContent.TableOfContents.AddAsync(tableOfContent);
        }

        public async Task DeleteTableOfContentAsync(int id)
        {
            SpatiumDbContent.TableOfContents.Remove(await GetTableOfContentByIdAsync(id));
        }

        public async Task<IEnumerable<TableOfContent>> GetTableOfContentAsync()
        {
            return await SpatiumDbContent.TableOfContents.ToListAsync();
        }

        public async Task<TableOfContent> GetTableOfContentByIdAsync(int id)
        {
            return await SpatiumDbContent.TableOfContents.FindAsync(id);
        }

        public async Task UpdateTableOfContentAsync(TableOfContent tableOfContent)
        {
            SpatiumDbContent.TableOfContents.Update(tableOfContent);
        }

        #endregion
    }
}
