using Autofac;
using WorldCupPool.Application.Common;
using WorldCupPool.Domain.Entities;
using WorldCupPool.Infrastructure.Auth;
using WorldCupPool.Infrastructure.Repository.AppUsers;
using WorldCupPool.Infrastructure.Repository.MatchResults;
using WorldCupPool.Infrastructure.Repository.Matches;
using WorldCupPool.Infrastructure.Repository.Predictions;

namespace WorldCupPool.Infrastructure.DependencyInjection
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JwtTokenGenerator>()
                .As<IJwtTokenGenerator>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BCryptPasswordHasher>()
                .As<IPasswordHasher>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AppUserRepository>()
                .As<IAppUserRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MatchRepository>()
                .As<IMatchRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MatchResultRepository>()
                .As<IMatchResultRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PredictionRepository>()
                .As<IPredictionRepository>()
                .InstancePerLifetimeScope();
        }
    }

}