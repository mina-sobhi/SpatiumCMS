using Infrastructure.Database.Database;

namespace Infrastructure.Database.Repository
{
    public class LookupRespositoryBase : RepositoryBase
    {
        public LookupRespositoryBase(SpatiumDbContent SpatiumDbContent) : base(SpatiumDbContent)
        {
        }
    }
}
