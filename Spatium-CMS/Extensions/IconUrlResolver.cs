using AutoMapper;
using Domain.LookupsAggregate;
using Spatium_CMS.Controllers.UserRoleController.Response;

namespace Spatium_CMS.Extensions
{
    public class IconUrlResolver : IValueResolver<RoleIcon, RoleIconRespones, string>
    {
        private readonly IConfiguration configration;
        public IconUrlResolver(IConfiguration configration)
        {
            this.configration = configration;
        }
        public string Resolve(RoleIcon source, RoleIconRespones destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.IconPath))
            {
                return $"{configration["ApiBaseUrl"]}/{source.IconPath}";
            }
            else
                return string.Empty;
        }
    }
}
