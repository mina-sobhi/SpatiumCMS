using Domain.Base;
using Domain.LookupsAggregate;

namespace Domain.ApplicationUserAggregate
{
    public class Audit : EntityBase
    {
        #region Properties
        public string ApplicationUserId { get; private set; }
        public string TableName { get; private set; }
        public int AffectedColumnsCount { get; private set; }
        public string RowKey { get; private set; }
        public string IconPath { get; private set; }
        public string OldValues { get; private set; }
        public string NewValues { get; private set; }
        public int AuditTypeId { get; private set; }
        #endregion

        #region Navigational Properties
        public virtual AuditType AuditType { get; private set; }
        public virtual ApplicationUser ApplicationUser { get; private set; }
        #endregion
    }
}
