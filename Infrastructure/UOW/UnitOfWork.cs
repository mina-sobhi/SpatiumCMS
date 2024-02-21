using Domain.ApplicationUserAggregate;
using Domain.BlogsAggregate;
using Domain.StorageAggregate;
using Domian.Interfaces;
using Infrastructure.Database.Database;
using Infrastructure.Database.Repository;
using Infrastructure.Database.Repository.StorageRepository;

namespace Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SpatiumDbContent _spatiumDbContent;
        private bool disposed = false;

        public IBlogRepository BlogRepository { get; }
        public IStorageRepository StorageRepository { get; }

        //public ITableOfContent TableOfContentRepository { get; }

        //public IPostRepository PostRepository { get; }

        //public ICommentRepository CommentRepository { get; }

        public IUserRoleRepository RoleRepository { get; }

        public UnitOfWork(SpatiumDbContent spatiumDbContent)
        {
            _spatiumDbContent = spatiumDbContent;

            #region Repos
            //Repo init goes Here
            BlogRepository= new BlogRepository(spatiumDbContent);
            //PostRepository = new PostRepository(spatiumDbContent);
            //TableOfContentRepository = new TableOfContentRepository(spatiumDbContent);
            //CommentRepository = new CommentRepository(spatiumDbContent);
            RoleRepository = new UserRoleReposiotry(spatiumDbContent);
            StorageRepository = new StorageRepository(spatiumDbContent);
            #endregion
        }

        public async Task SaveChangesAsync()
        {
            await _spatiumDbContent.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
                if (disposing)
                    _spatiumDbContent.Dispose();
            disposed = true;
        }
    }
}
