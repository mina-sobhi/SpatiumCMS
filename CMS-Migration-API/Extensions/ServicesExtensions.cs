using Infrastructure.Database.Database;
using Microsoft.EntityFrameworkCore;

namespace CMS_Migration_API.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SpatiumDbContent>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SpatiumCMS"),
                sqlServerOptionsAction: sqloption =>
                {
                    sqloption.MigrationsAssembly("Migrations");
                    sqloption.EnableRetryOnFailure();
                });
            });
        }
    }
}
