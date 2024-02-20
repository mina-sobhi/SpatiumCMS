using Domain.ApplicationUserAggregate;
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
    }
}
