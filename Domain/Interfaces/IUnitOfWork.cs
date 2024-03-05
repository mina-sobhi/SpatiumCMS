using Domain.ApplicationUserAggregate;
using Domain.BlogsAggregate;
using Domain.StorageAggregate;

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
        IStorageRepository StorageRepository { get; }
        #endregion
        Task SaveChangesAsync();
         IQueryable<Folder> GetFolderFamaily(int folderId);

    }
}
