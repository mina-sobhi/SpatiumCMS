using Domain.BlogsAggregate;
using Domain.StorageAggregate;
using Microsoft.EntityFrameworkCore.ChangeTracking;
namespace Infrastructure.Database.Helper
{
    public static class IconImageHelpers
    {
        public static int GetIconId(EntityEntry entity)
        {
            if (entity.Entity is Post)
                return 1;
            else if (entity.Entity is Folder)
                return 2;
            else
                return 3;
        }
    }
}
