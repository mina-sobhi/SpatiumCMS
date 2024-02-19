using Domain.Base;

namespace Domain.BlogsAggregate
{
    public interface IBlogRepository
    {
        #region Blog
        Task<IEnumerable<Blog>> FindAllAsync(int lastPageId, int NumberofRecord);
         Task<Blog> GetByIdAsync(int id);
        Task<IEnumerable<Blog>> GetBlogsAsync();
        Task CreateAsync(Blog blog);
        Task UpdateAsync(Blog blog);
        Task DeleteAsync(int id);
        #endregion
        #region Comment
        Task<Comment> GetCommentByIdAsync(int id);
        Task<IEnumerable<Comment>> GetCommentsAsync();
        Task CreateCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int id);
        #endregion

        #region Post
        Task<List<Post>> GetPostsAsync(GetEntitiyParams getPostParams, int blogId);
        Task<Post> GetPostByIdAsync(int id, int blogId);
        Task<Post> GetPostByOwnerId(string userId, int postId);
        Task<Post> PostSnippetPreview(int postId);
        Task CreatePostAsync(Post post);
        Task UpdatePostAsync(Post post);
        #endregion

        #region TableOfContent
        Task<TableOfContent> GetTableOfContentByIdAsync(int id);
        Task<IEnumerable<TableOfContent>> GetTableOfContentAsync();
        Task CreateTableOfContentAsync(TableOfContent tableOfContent);
        Task UpdateTableOfContentAsync(TableOfContent tableOfContent);
        Task DeleteTableOfContentAsync(int id);
        #endregion



    }
}
