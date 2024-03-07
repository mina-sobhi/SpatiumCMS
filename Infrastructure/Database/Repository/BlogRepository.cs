using Domain.Base;
using Domain.BlogsAggregate;
using Infrastructure.Database.Database;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

                public async Task<Blog> GetByIdAsync(int id)
                {
                    return await SpatiumDbContent.Blogs.FindAsync(id);
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

        public async Task<IEnumerable<Comment>> GetCommentsAsync(int postId,int blogId, string FilterColumn = null, string FilterValue = null)
        {

            var Post = await SpatiumDbContent.Posts.FirstOrDefaultAsync(p => p.BlogId == blogId && p.Id == postId);
            var query = Post.Comments.AsQueryable();

            if (!string.IsNullOrEmpty(FilterColumn) && !string.IsNullOrEmpty(FilterValue))
            {
                query = query.ApplyFilter(FilterColumn, FilterValue);
            }
            return query.ToList();
        }

        public async Task<Comment> GetCommentByIdAsync(int commentId)
        {
            return await SpatiumDbContent.Comments.FindAsync(commentId);
        }

        public void UpdateCommentAsync(Comment comment)
        {
            SpatiumDbContent.Comments.Update(comment);
        }

        public async Task<List<Post>> GetTotalComments(int blogId)
        {
            var topPosts = new List<Post>();

            var blog = await SpatiumDbContent.Blogs
                .Include(b => b.Posts)
                    .ThenInclude(p => p.Comments)
                .FirstOrDefaultAsync(b => b.Id == blogId);

            if (blog == null)
            {
                return topPosts;
            }
            foreach (var post in blog.Posts)
            {
                int commentsCount = CalculateCommentsCount(post);
            }
            topPosts = blog.Posts.OrderByDescending(p => p.Comments.Count).Take(5).ToList();

            return topPosts;
        }
        private int CalculateCommentsCount(Post post)
        {
            int commentsCount = 0;
            foreach (var comment in post.Comments)
            {
                CountComments(comment, out int commentCount);
                commentsCount += commentCount;
            }

            return commentsCount;
        }
        private void CountComments(Comment comment, out int commentCount)
        {
            commentCount = 1;
            foreach (var reply in comment.Comments)
            {
                CountComments(reply, out int replyCount);
                commentCount += replyCount;
            }
        }

        #endregion

        #region Post
        public async Task<Post> GetPostByIdAsync(int postId)
        {
            return await SpatiumDbContent.Posts.SingleOrDefaultAsync(p=> p.Id == postId);
        }
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

            if (!string.IsNullOrEmpty(postParams.FilterColumn))
            {
                if (!string.IsNullOrEmpty(postParams.FilterValue) && postParams.StartDate == null && postParams.EndDate == null)
                {
                    query = query.ApplyFilter(postParams.FilterColumn, postParams.FilterValue);
                }
                if (postParams.StartDate != null && postParams.EndDate != null && postParams.FilterColumn.ToLower() == "creationdate")
                {
                    query = query.Where(p => p.CreationDate.Date >= postParams.StartDate.Value && p.CreationDate.Date <= postParams.EndDate.Value);
                }
                if (postParams.StartDate != null && postParams.EndDate != null && postParams.FilterColumn.ToLower() == "lastupdate")
                {
                    query = query.Where(p => p.LastUpdate.Value >= postParams.StartDate.Value && p.LastUpdate.Value <= postParams.EndDate.Value);
                }

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

            return await paginatedQuery.ToListAsync();
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

        public void UpdatePost(Post post)
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
        //public async Task<IEnumerable<Post>> GetTopLikePost(int blogId, int count)
        //{
        //    return await SpatiumDbContent.Posts.Where(p=>p.CommentsAllowed== true).OrderByDescending(p => p.LikeCount).Take(count).ToListAsync();
        //}
        //public async Task<IEnumerable<Post>> GetTopSharePost(int blogId, int count)
        //{
        //    return await SpatiumDbContent.Posts.OrderByDescending(p => p.ShareCount).Take(count).ToListAsync();
        //}
        public async  Task<IEnumerable<Post>> GetAllPostByBlogId(int blogId)
        {
            return await SpatiumDbContent.Posts.ToListAsync();
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

        public void UpdateTableOfContentAsync(TableOfContent tableOfContent)
        {
            SpatiumDbContent.TableOfContents.Update(tableOfContent);
        }

        #endregion
        #region Like
        public async Task CreateLiketAsync(Like like)
        {
            await SpatiumDbContent.Likes.AddAsync(like);
        }
        public async Task<Like> GetLiketByPostIdAndCreatedByIdAsync(int postId, string userId)
        {
            return await SpatiumDbContent.Likes.SingleOrDefaultAsync(l => l.PostId == postId && l.CreatedbyId == userId);
        }

        public async Task DeleteLiketAsync(Like like)
        {
            SpatiumDbContent.Likes.Remove(await GetLiketByPostIdAndCreatedByIdAsync(like.PostId, like.CreatedbyId));
        }
        #endregion
        #region Share
        public async  Task CreateSharetAsync(Share share)
        {
            await SpatiumDbContent.Shares.AddAsync(share);
        }
        #endregion
    }
}
