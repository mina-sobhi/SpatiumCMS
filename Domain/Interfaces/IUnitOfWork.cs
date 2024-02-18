using Domain.ApplicationUserAggregate;
using Domain.BlogsAggregate;

namespace Domian.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        #region Repos
        public IBlogRepository BlogRepository { get; }
        //public ITableOfContent TableOfContentRepository { get; }
        //public IPostRepository PostRepository { get; }
        //public ICommentRepository CommentRepository { get; }
        public IUserRoleRepository RoleRepository { get; }
        #endregion
        Task SaveChangesAsync();
    }
}
