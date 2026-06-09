using Autofac;
using WorldCupPool.Application.Features.Auth;
using WorldCupPool.Application.Features.Leaderboard;
using WorldCupPool.Application.Features.Matches;
using WorldCupPool.Application.Features.Predictions;

namespace WorldCupPool.Application.DependencyInjection
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthService>()
                .As<IAuthService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MatchesService>()
                .As<IMatchesService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<LeaderboardService>()
                .As<ILeaderboardService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PredictionsService>()
                .As<IPredictionsService>()
                .InstancePerLifetimeScope();
        }
    }
}

