using Domain.ApplicationUserAggregate;
using Domain.BlogsAggregate;
using Domain.storageAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Database.Database
{
    public class SpatiumDbContent : IdentityDbContext<ApplicationUser, UserRole, string>
    {
        #region DBsets

        #region Application user Aggregate
        public DbSet<UserModule> UserModules { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<RolePermission> RoleModules { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
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
            #endregion

            #region Idintity-Configration Seeding-Data
            //var roles = new UserRole[3];
            //roles[0] = new UserRole() { Name = "SuperAdmin", NormalizedName = "SUPERADMIN",/* Description = "SuperAdmin Permission" */};
            //roles[1] = new UserRole() { Name = "Admin", NormalizedName = "ADMIN",/* Description = "Admin Permission" */};
            //roles[2] = new UserRole() { Name = "owner", NormalizedName = "OWNER", /*Description = "Owner Permission"*/ };

            //modelBuilder.Entity<UserRole>().HasData(roles);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
            #endregion
        }
    }
}
