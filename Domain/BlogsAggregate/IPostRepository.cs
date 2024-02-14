
using Domain.Base;
using Utilities.Enums;
namespace Domain.BlogsAggregate
{
    public interface IPostRepository
    {
        Task<List<Post>> GetPostsAsync(GetEntitiyParams getPostParams,int blogId);
        Task<Post> GetByIdAsync(int id);
        Task<Post> PostSnippetPreview(int postId);
        Task CreateAsync(Post post);
        Task UpdateAsync(Post post);
        Task DeleteAsync(int id);
    }
}
