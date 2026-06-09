using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WorldCupPool.Domain.Enums;
using WorldCupPool.Domain.Entities;
using WorldCupPool.Infrastructure.Auth;
using WorldCupPool.Infrastructure.Persistence;
using WorldCupPool.Infrastructure.Repository.AppUsers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WorldCupPool.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtOptions>>().Value);

            services.AddDbContext<AppDbContext>(opt =>
            {
                var connectionString = configuration.GetConnectionString("Default");
                opt.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 4, 0)));
            });

            services.AddScoped<IAppUserRepository, AppUserRepository>();


            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    var jwt = configuration.GetSection("Jwt").Get<JwtOptions>()!;

                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwt.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwt.Audience,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(2)
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthPolicies.Admin, p => p.RequireRole(UserRole.Admin.ToString()));
                options.AddPolicy(AuthPolicies.UserOrAdmin, p => p.RequireRole(UserRole.User.ToString(), UserRole.Admin.ToString()));
            });


            return services;
        }
    }
}

