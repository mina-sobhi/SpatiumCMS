using Domain.ApplicationUserAggregate;
using Domain.Base;
using Domain.BlogsAggregate.Input;

namespace Domain.BlogsAggregate
{
    public class Share : EntityBase
    {
        public bool IsShare { get; private set; }
        public string CreatedbyId { get; private set; }
        public int PostId { get; private set; }

        public virtual ApplicationUser Createdby { get; private set; }
        public virtual Post Post { get; private set; }
        #region Ctor
        public Share()
        {}
        public Share(ShareInput  input)
        {
            this.IsShare = true;
            this.CreatedbyId = input.CreatedbyId;
            this.PostId = input.PostId;
            this.CreationDate = DateTime.UtcNow;
            this.IsDeleted = false;
        }
        #endregion
        public void UnShare()
        {
            this.IsShare = false;
        }
    }
}
