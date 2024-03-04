using Domain.Base;
using System.Linq.Expressions;

namespace Domain.BlogsAggregate
{
    public interface IBlogRepository
    {
        #region Blog
        Task<IEnumerable<Blog>> FindAllAsync(int lastPageId, int NumberofRecord);
         Task<Blog> GetByIdAsync(int id);
        //Task<IEnumerable<Blog>> GetBlogsAsync();
        //Task CreateAsync(Blog blog);
        //Task UpdateAsync(Blog blog);
        //Task DeleteAsync(int id);
        #endregion

        #region Comment
        Task<Comment> GetCommentByIdAsync(int id);
        Task<IEnumerable<Comment>> GetCommentsAsync(int postId, int blogId, string FilterColumn, string FilterValue);
        Task CreateCommentAsync(Comment comment);
        void UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int id);
        #endregion

        #region Post
        Task<List<Post>> GetPostsAsync(GetEntitiyParams getPostParams, int blogId);
        Task<Post> GetPostByIdAsync(int id, int blogId);
        Task<Post> GetPostByOwnerId(string userId, int postId);
        Task<Post> PostSnippetPreview(int postId);
        Task CreatePostAsync(Post post);
        void UpdatePost(Post post);
        Task <Post> GetPostByExpression(Expression<Func<Post, bool>> expression);
        //Task<IEnumerable<Post>> GetTopLikePost(int blogId ,int count);
        //Task<IEnumerable<Post>> GetTopSharePost(int blogId, int count);
        Task<IEnumerable<Post>> GetAllPostByBlogId(int blogId);

        Task<Post> GetPostByIdAsync(int postId);
        #endregion

        #region TableOfContent
        Task<TableOfContent> GetTableOfContentByIdAsync(int id);
        Task<IEnumerable<TableOfContent>> GetTableOfContentAsync();
        Task CreateTableOfContentAsync(TableOfContent tableOfContent);
        void UpdateTableOfContentAsync(TableOfContent tableOfContent);
        Task DeleteTableOfContentAsync(int id);
        #endregion
        #region Like
        Task CreateLiketAsync(Like like);
        Task DeleteLiketAsync(Like like);
        Task<Like> GetLiketByPostIdAndCreatedByIdAsync(int postId , string userId);
        #endregion
        #region Share
        Task CreateSharetAsync(Share share);
        #endregion
    }
}
