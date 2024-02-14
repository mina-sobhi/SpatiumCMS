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
    }
}
