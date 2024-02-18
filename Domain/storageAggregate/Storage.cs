using Domain.ApplicationUserAggregate;
using Domain.Base;
using Domain.BlogsAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.storageAggregate
{
    public class Storage :EntityBase
    {
        #region Property

        public double Capacity { get; private set; }

        public int BlogId { get; private set; }

        public string ApplicationUserId { get; private set; }

        #endregion


        #region NavigationProperty
        public virtual ApplicationUser ApplicationUser { get; private set; }
        public virtual Blog Blog { get; private set; }
        #endregion

        #region List
        private readonly List<Folder> _folders = new List<Folder>();
        public virtual IReadOnlyList<Folder> Folders { get { return _folders.ToList(); } }
        #endregion

    }
}
