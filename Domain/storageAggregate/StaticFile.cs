using Domain.ApplicationUserAggregate;
using Domain.Base;
using Domain.BlogsAggregate;
using Domain.StorageAggregate.Input;

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
        public string?  Dimension { get; private set; }
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

        public void MoveToFolderId(int? FoldrId)
        {
            this.FolderId = FoldrId;
        }
        private void validations(string Name, string Description)
        {}
        #endregion
    }
}
