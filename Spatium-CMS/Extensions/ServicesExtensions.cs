using Domain.ApplicationUserAggregate;
using Domain.Interfaces;
using Domian.Interfaces;
using Infrastructure.Database.Database;
using Infrastructure.Database.Repository;
using Infrastructure.Services.AuthinticationService;
using Infrastructure.Services.MailSettinService;
using Infrastructure.Strategies.AuthorizationStrategy;
using Infrastructure.Strategies.PostStatusStrategy.Factory;
using Infrastructure.UOW;
using MailKit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Spatium_CMS.Extensions
{
    public static class ServicesExtensions
    {
        #region CORS Config
        public static void ConfigureCORS(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                var originsConfig = configuration.GetSection("AllowedCors").Value;
                var orginsArr = originsConfig?.Split(',', StringSplitOptions.RemoveEmptyEntries);

                if (originsConfig != "*" && (orginsArr?.Any() ?? false))
                {
                    options.AddPolicy("AllowSpecificOrigin",
                        builder => builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins(orginsArr));
                }
                else
                {
                    options.AddPolicy("AllowSpecificOrigin",
                        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                }
            });
        }
        #endregion

        #region Db Configs
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

        public static void ConfigureIdentityDbContext(this IServiceCollection services) =>
           services.AddIdentity<ApplicationUser, UserRole>(options =>
           {
               options.User.RequireUniqueEmail = true;
               options.Password.RequireNonAlphanumeric = true;
               options.Password.RequireUppercase = true;
               options.Password.RequireLowercase = true;
               options.Password.RequireDigit = true;
               options.Password.RequiredLength = 8;
           }).AddEntityFrameworkStores<SpatiumDbContent>()
             .AddDefaultTokenProviders();

        #endregion

        #region Configure Auth
        public static void ConfigureAuthentication(this IServiceCollection services, AuthConfig authConfig) =>
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = authConfig.ValidAudience,
                ValidIssuer = authConfig.ValidIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.SecretKey)),
                TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.EncryptionKey))
            };
        });

        #endregion

        #region Business Service
        public static void AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            var mailConfig=services.Configure<MailSetting>(configuration.GetSection("MailSettings"));
            if (mailConfig != null)
            {
                services.AddSingleton<ISendMailService, SendMailService>();
            }
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserRoleRepository, UserRoleReposiotry>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthorizationStrategyFactory, AuthorizationStrategyFactory>();
            services.AddScoped<IPostStatusFactory, PostStatusFactory>();

        }
        #endregion

        #region Configure Swagger Gen
        public static void AddSwaggerConfigs(this IServiceCollection services) =>
          services.AddSwaggerGen(c =>
          {
              c.SwaggerDoc("v1", new OpenApiInfo { Title = "API.PortalApp", Version = "v1" });
              
              #region JWT Token

              c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
              {
                  Description = @"Enter 'Bearer <token>' Example: 'Bearer 12345abcdef'",
                  Name = "Authorization",
                  In = ParameterLocation.Header,
                  Type = SecuritySchemeType.ApiKey,
                  Scheme = "Bearer"
              });

              c.AddSecurityRequirement(new OpenApiSecurityRequirement()
              {
                        {
                          new OpenApiSecurityScheme
                          {
                            Reference = new OpenApiReference
                              {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                              },
                              Scheme = "oauth2",
                              Name = "Bearer",
                              In = ParameterLocation.Header,

                            },
                            new List<string>()
                          }
                });

      #endregion
  });
        #endregion
    }
}
