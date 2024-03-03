using Domain.ApplicationUserAggregate;
using Domain.Base;

namespace Domain.BlogsAggregate
{
    public class Like 
    {
        public int Id { get; protected set; }
        public DateTime CreationDate { get; protected set; } = DateTime.UtcNow;
        public string CreatedbyId { get; private set; }
        public int PostId { get; private set; }

        public virtual ApplicationUser Createdby { get; private set; }
        public virtual Post Post { get; private set; }

        #region Ctor
        public Like()
        {
            
        }
        #endregion
    }
}
