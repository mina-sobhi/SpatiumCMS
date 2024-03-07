using Domain.ApplicationUserAggregate;
using Domain.Base;
using Infrastructure.Extensions.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Utilities.Enums;

namespace Infrastructure.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<ApplicationUser> FindUserInBlogAsync(this UserManager<ApplicationUser> userManager, int blogId, string userId)
        {
            return await userManager.Users.Where(x => x.BlogId == blogId && x.Id == userId).FirstOrDefaultAsync();
        }

        public static async Task<ApplicationUser> FindUserByEmailIgnoreFilter(this UserManager<ApplicationUser> userManager, string email)
        {
            return await userManager.Users.Include(r => r.Role)
                                          .ThenInclude(x => x.RolePermission)
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

        public static async Task<List<ApplicationUser>> FindUsersInBlogIncludingRole(this UserManager<ApplicationUser> userManager, int blogId, GetEntitiyParams entityParams)
        {
            var query = userManager.Users.Include(x => x.Role).Where(x => x.BlogId == blogId).AsQueryable();
            if (!string.IsNullOrEmpty(entityParams.FilterColumn))
            {
                if (!string.IsNullOrEmpty(entityParams.FilterValue) && entityParams.StartDate == null && entityParams.EndDate == null)
                {
                    query = query.ApplyFilter(entityParams.FilterColumn, entityParams.FilterValue);
                }
                if (entityParams.StartDate != null && entityParams.EndDate != null && entityParams.FilterColumn.ToLower() == "createdat")
                {
                    query = query.Where(p => p.CreatedAt.Date >= entityParams.StartDate.Value && p.CreatedAt.Date <= entityParams.EndDate.Value);
                }
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

            return await paginatedQuery.ToListAsync();

        }

        public static async Task<UsersAnalytics> GetBlogUersAnalytics(this UserManager<ApplicationUser> userManager, int blogId)
        {
            var allUsers = userManager.Users.Where(x => x.BlogId == blogId);
            var TotalCount = await allUsers.CountAsync();
            var ActivateUsers = await allUsers.Where(u=>u.UserStatusId==(int)UserStatusEnum.Active).CountAsync();
            var DeActivateUsers = await allUsers.Where(u => u.UserStatusId == (int)UserStatusEnum.DeActive).CountAsync();
            return new UsersAnalytics()
            {
                TotalCount = TotalCount,
                ActivateUsers = ActivateUsers,
                DeactivateUsers = DeActivateUsers
            };
        }
    }
}
