using Domain.ApplicationUserAggregate;
using Domain.ApplicationUserAggregate.Inputs;
using Domain.Base;
using Domain.BlogsAggregate;
using Domain.LookupsAggregate;
using Domain.StorageAggregate;
using Infrastructure.Database.Helper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;
using System.Text;

namespace Infrastructure.Database.Database
{
    public class SpatiumDbContent : IdentityDbContext<ApplicationUser, UserRole, string>
    {
        #region DBsets

        #region Application user Aggregate
        public DbSet<UserModule> UserModules { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        
        #endregion

        #region Blog Aggregate
        //public DbSet<Audit> Audits { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<TableOfContent> TableOfContents { get; set; }
        #endregion

        #region Storage Aggregate
        public DbSet<Storage> Storages { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<StaticFile> Files { get; set; }
        #endregion

        #region Lookups
        public DbSet<RoleIcon> RoleIcons { get; set; }
        public DbSet<CommentStatus> CommentStatuses { get; set; }
        public DbSet<PostStatus> PostStatuses { get; set; }
        public DbSet<LogIcon> LogIcons { get; set; }

        #endregion


        #endregion

        public SpatiumDbContent(DbContextOptions<SpatiumDbContent> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Entities Relationship
            foreach (var relation in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relation.DeleteBehavior = DeleteBehavior.Restrict;
            }
            #endregion

            modelBuilder.Entity<Post>().HasMany(x => x.TableOfContents).WithOne(x => x.Post).HasForeignKey(x => x.PostId);
            modelBuilder.Entity<TableOfContent>().HasMany(x => x.ChildTableOfContents).WithOne(x => x.ParentTableOfContent).HasForeignKey(x => x.ParentTableOfContentId);

            #region Global Filter
            modelBuilder.Entity<Post>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<TableOfContent>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Comment>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<ApplicationUser>().HasQueryFilter(x => x.IsAccountActive);
            modelBuilder.Entity<UserRole>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<UserPermission>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<RolePermission>().HasQueryFilter(x => !x.IsDeleted);
            #endregion

            #region Idintity-Configration
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
            #endregion
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //var ModifiedEntity = ChangeTracker.Entries<EntityBase>()
            //    .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added || e.State == EntityState.Deleted)
            //    .ToList();
            //foreach (var entry in ModifiedEntity)
            //{
            //    var ActivityLogInput = new ActivityLogInput();
            //    ActivityLogInput.UserId = entry.Property("CreatedById")?.CurrentValue != null ? entry.Property("CreatedById").CurrentValue.ToString() : "Null";
            //    ActivityLogInput.Content = GetChanges(entry); 
            //    ActivityLogInput.LogIconId = IconImageHelpers.GetIconId(entry);
            //    ActivityLogs.Add(new ActivityLog(ActivityLogInput));
            //}
           
            return base.SaveChangesAsync(cancellationToken);
        }
        private static string GetChanges(EntityEntry entity)
        {
            var changes = new StringBuilder();
            var tblName = entity.Entity.GetType(). Name;
            switch (entity.State)
            {
                case EntityState.Deleted :
                    changes.Append($"{tblName} Is Deleted");
                    break;
                case EntityState.Modified:
                    if(Convert.ToBoolean(entity.Property("IsDeleted").CurrentValue) == true)
                    {
                        changes.Append($"{tblName} Is Deleted");
                        break;
                    }
                    changes.Append($"{tblName} Is Modified");
                    break;
                case EntityState.Added:
                    changes.Append($" New {tblName} Is Added ");
                    break;
                default:
                    break;
            }

            return changes.ToString();
        }
    }
}
