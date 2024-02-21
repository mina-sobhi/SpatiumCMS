using Domain.ApplicationUserAggregate;
using Domain.Base;
using Domain.BlogsAggregate.Input;
using Domain.StorageAggregate;
using Domain.StorageAggregate.Input;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.BlogsAggregate
{
    public class Blog:EntityBase
    {
        #region CTOR
        public Blog()
        {

        }

        public Blog(BlogInput blogInput)
        {
            Name = blogInput.Name;
            FavIconPath = blogInput.FavIconPath;
        }
        #endregion

        #region Properties
        public string Name { get; private set; }
        public string FavIconPath { get; private set; }
        //public int StorageId { get; private set; }

        #endregion

        #region Navigational Properties
        //public virtual Storage Storage { get; private set; }
        #endregion

        #region Virtual Lists
        private readonly List<Post> _posts = new();
        public virtual IReadOnlyList<Post> Posts=> _posts.ToList();
        #endregion
    }
}
