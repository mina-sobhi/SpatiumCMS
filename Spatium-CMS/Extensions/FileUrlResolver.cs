using AutoMapper;
using Domain.StorageAggregate;
using Spatium_CMS.Controllers.StorageController.Response;

namespace Spatium_CMS.Extensions
{
    public class FileUrlResolver : IValueResolver<StaticFile, ViewFile, string>
    {
        private readonly IConfiguration configration;
        public FileUrlResolver(IConfiguration configration)
        {
            this.configration = configration;
        }
        public string Resolve(StaticFile source, ViewFile destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.UrlPath))
            {
                return $"{configration["ApiBaseUrl"]}/{source.UrlPath}";
            }
            else
                return string.Empty;
        }
    }
}
