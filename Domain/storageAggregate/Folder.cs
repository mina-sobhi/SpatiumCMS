using Domain.ApplicationUserAggregate;
using Domain.Base;
using Domain.BlogsAggregate;
using Domain.StorageAggregate.Input;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Exceptions;

namespace Domain.StorageAggregate
{
    public class Folder : EntityBase
    {
        #region Property

        // virtual name 
        public string Name { get; private set; }
        public  string Description { get; set; }
        public int StorageId { get; private set; }
        public int BlogId { get; private set; }
        [ForeignKey(nameof(Parent))]
        public int? ParentId { get; private set; }
        [ForeignKey(nameof(CreatedBy))]
        public string CreatedById { get; private set; }
        #endregion

        #region NavigationProperty
        public virtual Storage Storage { get; private set; }
        public virtual Blog Blog { get; private set; }
        public virtual Folder Parent { get; private set; }
        public virtual ApplicationUser CreatedBy { get; private set; }
        #endregion

        #region List
        private readonly List<StaticFile> _files = new List<StaticFile>();
        public virtual IReadOnlyList<StaticFile> Files { get { return _files.ToList(); } }
        private readonly List<Folder> _folders = new List<Folder>();
        public virtual IReadOnlyList<Folder> Folders { get { return _folders.ToList(); } }
        #endregion

        #region Ctor
        public Folder()
        {}
        public Folder(AddFolderInput input)
        {
          
            validations(input.Name, input.Description);
            this.Name = input.Name; 
            this.Description = input.Description;
            this.ParentId = input.ParentId is null ? null : input.ParentId.Value;
            this.BlogId = input.BlogId;
            this.CreatedById = input.CreatedById;
            this.CreationDate = DateTime.UtcNow;
            this.StorageId = input.StorageId;
            this.IsDeleted = false;
        }
        #endregion

        #region Method
        public void Update(updateFolderInput input)
        {
           
            validations(input.Name , input.Description);
            this.ParentId = input.ParentId is null ? null : input.ParentId.Value;
            this.Name = input.Name;
            this.Description = input.Description;   
        }
        public void Delete()
        {
            this.IsDeleted = true;
            foreach (var file in this._files)
            {
                file.Delete();
            }
            foreach (var subFolder in this._folders)
            {
                subFolder.IsDeleted=true;
                foreach (var file in subFolder.Files)
                {
                    file.Delete();
                }
            }
        }
        public void Rename(string newName)
        {
            this.Name = Name.Length < 2 || Name.Length > 50 ? throw new SpatiumException("Folder Name Must in Range 2 to 50 char ") : newName  ;
        }
        public void MoveTo(int? DestinationId)
        {
            this.ParentId= DestinationId;
            foreach (var file in this._files)
            {
                file.MoveToFolderId(this.Id);
            }
        }
        private void validations(string Name , string Description)
        {
           
            if (Name.Length < 2 || Name.Length > 50)
            { 
                throw new SpatiumException("Folder Name Must in Range 2 to 50 char "); 
            } 
            if (Description.Length < 20  || Description.Length > 200)
            {
                throw new SpatiumException("Folder Description Must in Range 20 to 200 char ");
            }
        }
        #endregion
    }
}
