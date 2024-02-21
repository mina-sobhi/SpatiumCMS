using Domain.ApplicationUserAggregate;
using Domain.Base;
using Domain.BlogsAggregate;
using Domain.StorageAggregate.Input;
using Utilities.Exceptions;

namespace Domain.StorageAggregate
{
    public class StaticFile : EntityBase
    {
        #region Properts
        public string Name { get; private set; }
        public string Extention { get; private set; }
        //test
        public string UrlPath { get; private set; }
        public string Caption { get; private set; }
        public string FileSize { get; private set; }
        public string Alt { get; private set; }
        public string  Dimension { get; private set; }
        public string CreatedById { get; private set; }
        public int? FolderId { get; private set; }
        public int BlogId { get; private set; }
        #endregion

        #region NavigationProperty
        public virtual Folder Folder { get; private set; }
        public virtual Blog Blog { get; private set; }
        public virtual ApplicationUser CreatedBy { get; private set; }
        #endregion

        #region Ctor
        public StaticFile()
        {
            
        }
        public StaticFile(FileInput input)
        {
            validations(input.Name, input.Caption, input.Alt);
            ExtensionValidations(input.Extention);
            
            this.CreationDate = DateTime.UtcNow;
            this.FolderId = input.FolderId;
            this.CreatedById = input.CreatedById;
            this.BlogId = input.BlogId;
            this.Name = input.Name;
            this.Extention = input.Extention;
            this.Caption = input.Caption;
            this.FileSize = input.FileSize;
            this.Alt = input.Alt;
            this.Dimension = input.Dimension;
            this.UrlPath = input.UrlPath;
        }
        #endregion

        #region Method
        public void Update(FileInput input)
        {
            this.Name = input.Name;
            this.Extention = input.Extention;
            this.Caption = input.Caption;
            this.FileSize = input.FileSize;
            this.Alt = input.Alt;
            this.Dimension = input.Dimension;
        }
        public void Delete()
        {
            this.IsDeleted = true;
        }
        private void validations(string Name, string Caption, string Alt)
        {

            if (Name.Length < 2 && Name.Length > 50)
            {
                throw new SpatiumException("File Name Must in Range 2 to 50 char ");
            }
            if (Caption.Length < 20 && Caption.Length > 200)
            {
                throw new SpatiumException("File Description Must in Range 20 to 200 char ");
            }
            if (Alt.Length < 5 && Alt.Length > 60)
            {
                throw new SpatiumException("File Alt Must in Range 5 to 60 char ");
            }
        }

        private void ExtensionValidations(string Extension)
        {
            //var VideoExtensions = new List<string> { "mp4", "3g2", "3gp", "wmv", "webm", "m4v" };
            //var RecordExtensions = new List<string> { "mp3", "mpa", "wma", "wma", "aif", "cda" };
            //var ImageExtensions = new List<string> { "jpg", "png", "webp", "gif", "bin" };
            //var DocumentExtensions = new List<string> { "csv", "xlsx", "xls", "doc", "docs", "pdf", "txt", "xml" };
            var cleanExtension = Extension.ToLower();
            var extentions = new List<string> { "mp4", "3g2", "3gp", "wmv", "webm", 
                "m4v", "mp3", "mpa", "wma", "wma", "aif", "cda",
            "jpg", "png", "webp", "gif", "bin" ,
            "csv", "xlsx", "xls", "doc", "docs", "pdf", "txt", "xml"};
            var flag = extentions.Any(x=>x.Equals(Extension));
            if (!flag)
            {
                throw new SpatiumException("Invalid Extentions");
            }
        }
        #endregion
    }
}
