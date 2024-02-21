using Domain.ApplicationUserAggregate;
using Domain.Base;
using Domain.BlogsAggregate;
using Domain.StorageAggregate.Input;

namespace Domain.StorageAggregate
{
    public class Storage :EntityBase
    {
        #region Property
        public string Capacity { get; private set; }
        public int BlogId { get; private set; }
        public string ApplicationUserId { get; private set; }

        #endregion

        #region NavigationProperty
        public virtual ApplicationUser ApplicationUser { get; private set; }
        public virtual Blog Blog { get; private set; }
        #endregion

        #region List
        private readonly List<Folder> _folders = new List<Folder>();
        public virtual IReadOnlyList<Folder> Folders => _folders.ToList();
        #endregion

        #region Ctor
        public Storage()
        {}
        public Storage(StorageInput input)
        {
            this.IsDeleted  = false;
            this.CreationDate = DateTime.UtcNow;
            this.Capacity = input.Capacity;
            this.BlogId = input.BlogId;
            this.ApplicationUserId = input.ApplicationUserId;
        }
        #endregion



    }
}
