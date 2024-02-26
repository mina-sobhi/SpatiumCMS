using Domain.ApplicationUserAggregate.Inputs;
using Domain.Base;
using Domain.LookupsAggregate;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.ApplicationUserAggregate
{
    public class ActivityLog:EntityBase
    {
        #region Prop
        public string UserId { get; private set; }
        public string Content { get; private set; }
        public int LogIconId { get; private set; }
        #endregion

        #region CTOR
        public ActivityLog() { }
        public ActivityLog(ActivityLogInput activityLogInput)
        {
            this.CreationDate = DateTime.UtcNow;
            this.IsDeleted = false;
            UserId = activityLogInput.UserId;
            Content = activityLogInput.Content;
            LogIconId = activityLogInput.LogIconId;
          
        } 
        #endregion

        #region Navigational Prop
        public virtual ApplicationUser User { get; private set; }
       
        public virtual LogIcon LogIcon { get; private set; }
        #endregion

    }
}
