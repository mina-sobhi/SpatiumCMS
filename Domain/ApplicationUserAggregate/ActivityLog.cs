using Domain.ApplicationUserAggregate.Inputs;
using Domain.Base;

namespace Domain.ApplicationUserAggregate
{
    public class ActivityLog:EntityBase
    {
        #region CTOR
        public ActivityLog() { }
        public ActivityLog(ActivityLogInput activityLogInput)
        {
            UserId = activityLogInput.UserId;
            Content = activityLogInput.Content;
            IconPath = activityLogInput.IconPath;
        } 
        #endregion

        #region Prop
        public string UserId { get; private set; }
        public string Content { get; private set; }
        public string IconPath { get; private set; }
        #endregion

        #region Navigational Prop
        public virtual ApplicationUser User { get; private set; }
        #endregion
    }
}
