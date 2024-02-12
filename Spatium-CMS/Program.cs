using Infrastructure.Services.AuthinticationService;
using Spatium_CMS.AutoMapperProfiles;
using Spatium_CMS.Extensions;

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

app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
