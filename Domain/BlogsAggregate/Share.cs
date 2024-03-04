
namespace Domain.BlogsAggregate
{
    public class Share 
    {
        public int Id { get; private set; }
        public DateTime CreationDate { get; private set; } = DateTime.UtcNow;
        public int PostId { get; private set; }

        public virtual Post Post { get; private set; }
        #region Ctor
        public Share()
        {}
        public Share(int postId)
        {
            this.PostId = postId;
        }
        #endregion
    }
}
