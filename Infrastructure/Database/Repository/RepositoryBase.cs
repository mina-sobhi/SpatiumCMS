using Infrastructure.Database.Database;

namespace Infrastructure.Database.Repository
{
    public abstract class RepositoryBase
    {
        protected SpatiumDbContent SpatiumDbContent { get; set; }
        public RepositoryBase(SpatiumDbContent SpatiumDbContent) { 
            this.SpatiumDbContent = SpatiumDbContent;
        }
    }
}
