using Domain.ApplicationUserAggregate;
using Domain.Base;
using Domain.BlogsAggregate;
namespace Domain.storageAggregate
{
    public class StaticFile : EntityBase
    {
        #region Properts
        public string Name { get; private set; }
        public string Extention { get; private set; }
        public string Caption { get; private set; }
        public string FileSize { get; private set; }
        public string CreatedById { get; private set; }
        public int? FolderId { get; private set; }
        public int BlogId { get; private set; }
        #endregion

        #region NavigationProperty
        public virtual Folder Folder { get; private set; }
        public virtual Blog Blog { get; private set; }
        public virtual ApplicationUser CreatedBy { get; private set; }

        #endregion
    }
}
