using AutoMapper;
using Domain.ApplicationUserAggregate;
using Spatium_CMS.Controllers.UserManagmentController.Response;

namespace Spatium_CMS.Extensions
{
    public class UserProfileResolver : IValueResolver<ApplicationUser, ViewUsersResponse, string>
    {
        private readonly IConfiguration configration;
        public UserProfileResolver(IConfiguration configration)
        {
            this.configration = configration;
        }
        public string Resolve(ApplicationUser source, ViewUsersResponse destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ProfileImagePath))
            {
                return $"{configration["ApiBaseUrl"]}/{source.ProfileImagePath}";
            }
            else
                return string.Empty;
        }
    }
}
