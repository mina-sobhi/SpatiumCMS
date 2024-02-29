using Domain.ApplicationUserAggregate;
using Domain.Base;
using Domain.BlogsAggregate.Input;

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

        #endregion

        #region Navigational Properties
        #endregion

        #region Virtual Lists
        private readonly List<Post> _posts = new();
        public virtual IReadOnlyList<Post> Posts=> _posts.ToList();

        private readonly List<ApplicationUser> _users = new();
        public virtual IReadOnlyList<ApplicationUser> Users => _users.ToList();
        #endregion
    }
}
