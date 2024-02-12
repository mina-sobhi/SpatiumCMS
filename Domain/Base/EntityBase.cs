namespace Domain.Base
{
    public abstract class EntityBase
    {
        public int Id { get; protected set; }
        public DateTime CreationDate { get; protected set; } = DateTime.UtcNow;
        public bool IsDeleted { get; protected set; } = false;
    }
}
