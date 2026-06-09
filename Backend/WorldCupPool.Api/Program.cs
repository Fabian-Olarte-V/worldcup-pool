using Autofac;
using Autofac.Extensions.DependencyInjection;
using WorldCupPool.Api.Middlewares;
using WorldCupPool.Application.Common;
using WorldCupPool.Application.DependencyInjection;
using WorldCupPool.Infrastructure.DependencyInjection;
using WorldCupPool.Infrastructure.Persistence;
using WorldCupPool.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

// Add services and modules to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new InfrastructureModule()));
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new ApplicationModule()));

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddTransient<GlobalExceptionMiddleware>();
builder.Services.AddTransient<ApiResponseWrapperMiddleware>();


var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

    await DbSeeder.SeedAsync(db, passwordHasher);
}

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<ApiResponseWrapperMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
