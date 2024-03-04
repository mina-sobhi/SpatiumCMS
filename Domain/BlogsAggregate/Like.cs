using Domain.ApplicationUserAggregate;

using Domain.BlogsAggregate.Input;

namespace Domain.BlogsAggregate
{
    public class Like 
    {
        public int Id { get; private set; }
        public DateTime CreationDate { get; private set; } = DateTime.UtcNow;
        public string CreatedbyId { get; private set; }
        public int PostId { get; private set; }

        public virtual ApplicationUser Createdby { get; private set; }
        public virtual Post Post { get; private set; }

        #region Ctor
        public Like()
        {
        }
        public Like(LikeInput likeInput)
        {
            this.PostId = likeInput.PostId;
            this.CreatedbyId = likeInput.CreatedbyId;
        }
        #endregion
    }
}
