using AutoMapper;
using Domain.ApplicationUserAggregate;
using Spatium_CMS.Controllers.AuthenticationController.Response;
namespace Spatium_CMS.Extensions
{
    public class UserProfileDetailesResolver : IValueResolver<ApplicationUser, ViewUserProfileResult, string>
    {
        private readonly IConfiguration configration;
        public UserProfileDetailesResolver(IConfiguration configration)
        {
            this.configration = configration;
        }
        public string Resolve(ApplicationUser source, ViewUserProfileResult destination, string destMember, ResolutionContext context)
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
