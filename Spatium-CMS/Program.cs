using Domain.ApplicationUserAggregate;
using Infrastructure.Services.AuthinticationService;
using Microsoft.AspNetCore.Identity;

using Infrastructure.Database.Database;
using Infrastructure.Services.AuthinticationService;
using Microsoft.EntityFrameworkCore;
using Spatium_CMS.AutoMapperProfiles;
using Spatium_CMS.Extensions;
using Spatium_CMS.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddControllers();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureIdentityDbContext();
builder.Services.ConfigureCORS(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddBusinessServices(builder.Configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerConfigs();

var authConfig = builder.Configuration.GetSection("AuthConfig").Get<AuthConfig>();
if(authConfig != null)
{
    builder.Services.AddSingleton(authConfig);
    builder.Services.ConfigureAuthentication(authConfig);
}


var app = builder.Build();

#region Auto Migration
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var ILoggerFactory = services.GetRequiredService<ILoggerFactory>();
        try
        {
            var dbcontext = services.GetRequiredService<SpatiumDbContent>();
            await dbcontext.Database.MigrateAsync();
            await DataSeedingService.Dataseeding(dbcontext);
        }
        catch (Exception ex)
        {
            var logger = ILoggerFactory.CreateLogger<Program>();
            logger.LogError(ex, "there is some thing wrong.....");
        }
    }
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.EnableDeepLinking();
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
//(UserManager<ApplicationUser>) WebApplication.ApplicationServices.GetService(typeof(UserManager<ApplicationUser>));
app.UseMiddleware<ValidateTokenMiddleware>();
app.MapControllers();
app.Run();
