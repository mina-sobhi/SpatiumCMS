using Domain.ApplicationUserAggregate;
using Domain.Base;
using Domain.BlogsAggregate.Input;

namespace Domain.BlogsAggregate
{
    public class Like : EntityBase
    {
        #region Property
        public bool IsLike { get; private set; }
        public string CreatedbyId { get; private set; }
        public int PostId { get; private set; }
        #endregion
        #region Navigation Prop 
        public virtual ApplicationUser Createdby { get; private set; }
        public virtual Post Post { get; private set; }
        #endregion
        #region Ctor
        public Like()
        {}
        public Like(LikeInput input)
        {
            this.IsLike = true;
            this.CreatedbyId = input.CreatedbyId;
            this.PostId = input.PostId;
            this.CreationDate = DateTime.UtcNow;
            this.IsDeleted = false;
        }
        #endregion
        public void DisLike()
        {
            this.IsLike = false;
        }


    }
}
