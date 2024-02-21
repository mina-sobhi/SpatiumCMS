using AutoMapper;
using Domain.StorageAggregate;
using Utilities.Exceptions;

namespace Spatium_CMS.AttachmentService
{
    public class AttachmmentService : IAttachmentService
    {
        private readonly IConfiguration configration;

        public AttachmmentService()
        {

        }
        public AttachmmentService(IConfiguration configration)
        {
            this.configration = configration;
        }
        public async Task<string> SaveAttachment(string dirctoryDestination, IFormFile formfile, string source, 
            string imageName)
        {

            string filePath = source + "\\" + dirctoryDestination;
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
            string url = dirctoryDestination + "/" + fileName;
            return url;
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
        public string GetDesireFileName(IFormFile file, string desiredFileName)
        {

            var originalFileName = file.FileName;
            var fileExtension = Path.GetExtension(originalFileName);
            var desiredFileNameWithExtension = $"{desiredFileName}{fileExtension}";
            return desiredFileNameWithExtension;
        }

        public void CheckFileExtension(IFormFile file, string Extension)
        {
            var originalFileName = file.FileName;
            var fileExtension = Path.GetExtension(originalFileName);
            var cleanFileExtension = fileExtension.TrimStart('.').ToLower();
            var cleanProvidedExtension = Extension.ToLower();
            if (!cleanProvidedExtension.Equals(cleanFileExtension, StringComparison.OrdinalIgnoreCase))
            {
                throw new SpatiumException("The Extention you entered are not the same");
            }
        }
    }
}
