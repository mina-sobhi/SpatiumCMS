using AutoMapper;
using Domain.StorageAggregate;

namespace Spatium_CMS.AttachmentService
{
    public class AttachmentService
    {
        private readonly IConfiguration configration;

        public AttachmentService(IConfiguration configration)
        {
            this.configration = configration;
        }
        public async Task<string> SaveAttachment(string dirctoryDestination, IFormFile formfile, string source, 
            string imageName)
        {

            string filePath = source + "/" + dirctoryDestination;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fileName = $"{imageName}{Path.GetExtension(formfile.FileName)}";
            string ImagePath = Path.Combine(filePath, fileName);
            using (FileStream stream = File.Create(ImagePath))
            {
                await formfile.CopyToAsync(stream);
            }
            return ImagePath;
        }


        public string Resolve(StaticFile source, FileResponse destination, string destMember, ResolutionContext context)
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
