﻿using Domain.ApplicationUserAggregate;
using Domain.ApplicationUserAggregate.Inputs;
using Domain.BlogsAggregate;
using Domain.StorageAggregate;
using Domian.Interfaces;
using Infrastructure.Database.Database;
using Infrastructure.Database.Repository;
using Infrastructure.Database.Repository.StorageRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

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
        //public void BeforeSaveChanages(string UserId)
        //{
            
        //    var activityLogs = new List<ActivityLog>();
        //    _spatiumDbContent.ChangeTracker.DetectChanges();
        //    foreach (var entry in _spatiumDbContent.ChangeTracker.Entries())
        //    {
        //        if (entry.Entity is ActivityLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
        //        {
        //            continue;
        //        }
                
        //        var name = entry.GetType().Name;
        //        var content = string.Empty;
        //        int iconId = 0;
        //        switch (entry.State)
        //        {
        //            case EntityState.Added :
        //                content = $" Add In {name} ";
        //                iconId = 1;
        //                break;
        //            case EntityState.Modified :
        //                content = $" Modify In {name} ";
        //                iconId = 2;
        //                break;
        //            case EntityState.Deleted :
        //                content = $" Modify In {name} ";
        //                iconId = 3;
        //                break;
        //            default:
        //                break;
        //        }
        //        if(content.Length> 0 &&  iconId > 0)
        //        {
        //            var activitylogInput = new ActivityLogInput()
        //            {
        //                UserId = UserId,
        //                LogIconId = iconId,
        //                Content = content,
        //            };
        //            activityLogs.Add(new ActivityLog(activitylogInput));
        //        }

            //}
        }
    }

