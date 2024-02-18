using Domain.ApplicationUserAggregate;
using Domain.Base;
using Domain.BlogsAggregate;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.storageAggregate
{
    public class Folder : EntityBase
    {
        #region Property

        // virtual name 
        public string Name { get; private set; }
        public int StorageId { get; private set; }
        public int BlogId { get; private set; }
        [ForeignKey(nameof(parent))]
        public int? ParentId { get; private set; }
        [ForeignKey(nameof(CreatedBy))]
        public string CreatedById { get; private set; }
        #endregion

        #region NavigationProperty
        public virtual Storage Storage { get; private set; }
        public virtual Blog Blog { get; private set; }
        public virtual Folder? parent { get; private set; }
        public virtual ApplicationUser CreatedBy { get; private set; }
        #endregion
        #region List
        private readonly List<StaticFile> _files = new List<StaticFile>();
        public virtual IReadOnlyList<StaticFile> Files { get { return _files.ToList(); } }
        private readonly List<Folder> _folders = new List<Folder>();
        public virtual IReadOnlyList<Folder> Folders { get { return _folders.ToList(); } }
        #endregion
    }
}
