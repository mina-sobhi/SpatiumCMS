using Domain.LookupsAggregate;
using Infrastructure.Database.Database;
using System.Text.Json;

namespace Spatium_CMS.Extensions
{
    public static class DataSeedingService
    {
        public static async Task Dataseeding(SpatiumDbContent context)
        {
            if (!context.Set<RoleIcon>().Any())
            {
                var IcoData = File.ReadAllText("Spatium-CMS/wwwroot/RoleIcon/RoleIconDataSeeding.json");
                var Icons = JsonSerializer.Deserialize<List<RoleIcon>>(IcoData);
                if (Icons?.Count > 0 && Icons is not null)
                {
                    foreach (var Icon in Icons)
                    {
                        await context.Set<RoleIcon>().AddAsync(Icon);
                    }
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}

