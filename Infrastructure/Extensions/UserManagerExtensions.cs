using Domain.ApplicationUserAggregate;
using Domain.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<ApplicationUser> FindUserInBlogAsync(this UserManager<ApplicationUser> userManager,int blogId, string userId)
        {
            return await userManager.Users.Where(x=>x.BlogId==blogId && x.Id==userId).FirstOrDefaultAsync();
        }

        public static async Task<ApplicationUser> FindUserByEmailIgnoreFilter(this UserManager<ApplicationUser> userManager, string email)
        {
            return await userManager.Users.Include(r=>r.Role)
                                          .ThenInclude(x=>x.RolePermission)
                                          .IgnoreQueryFilters()
                                          .FirstOrDefaultAsync(x => x.Email == email);
        }
        public static async Task<ApplicationUser> FindUserByIdInBlogIgnoreFilterAsync(this UserManager<ApplicationUser> userManager, int blogId, string userId)
        {
            return await userManager.Users.IgnoreQueryFilters().Where(x => x.BlogId == blogId && x.Id == userId).FirstOrDefaultAsync();
        }
        public static async Task<ApplicationUser> FindUserInBlogByIdIncludingRole(this UserManager<ApplicationUser> userManager, int blogId, string userId)
        {
            return await userManager.Users.Include(x => x.Role).Where(x => x.BlogId == blogId && x.Id == userId).FirstOrDefaultAsync();
        }

        public static async Task<List<ApplicationUser>> FindUsersInBlogIncludingRole(this UserManager<ApplicationUser> userManager, int blogId ,GetEntitiyParams entityParams)
        {
            var query =  userManager.Users.Include(x => x.Role).Where(x => x.BlogId == blogId).AsQueryable();

            if (!string.IsNullOrEmpty(entityParams.FilterColumn) && !string.IsNullOrEmpty(entityParams.FilterValue))
            {
                query = query.ApplyFilter(entityParams.FilterColumn, entityParams.FilterValue);
            }

            if (!string.IsNullOrEmpty(entityParams.SortColumn))
            {
                query = query.ApplySort(entityParams.SortColumn, entityParams.IsDescending);
            }

            if (!string.IsNullOrEmpty(entityParams.SearchColumn) && !string.IsNullOrEmpty(entityParams.SearchValue))
            {
                query = query.ApplySearch(entityParams.SearchColumn, entityParams.SearchValue);
            }

            var paginatedQuery = query.Skip((entityParams.Page - 1) * entityParams.PageSize).Take(entityParams.PageSize);

            return paginatedQuery.ToList();
            
        }

        public static async Task<UsersAnalytics> GetBlogUersAnalytics(this UserManager<ApplicationUser> userManager, int blogId)
        {
            var allUsers=  await userManager.Users.Include(x => x.Role).Where(x => x.BlogId == blogId).ToListAsync();
            var TotalCount=allUsers.Count();
            var ActivateUsers =allUsers.Where(u=>u.IsAccountActive==true).Count();
            var DeActivateUsers =allUsers.Where(u=>u.IsAccountActive==true).Count();
            return new UsersAnalytics()
            {
                TotalCount = TotalCount,
                ActivateUsers = ActivateUsers,
                DeactivateUsers = DeActivateUsers

            };

        }
    }
}
