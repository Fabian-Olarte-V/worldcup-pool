using Microsoft.EntityFrameworkCore;
using WorldCupPool.Application.Common;
using WorldCupPool.Domain.Entities;
using WorldCupPool.Domain.Enums;

namespace WorldCupPool.Infrastructure.Persistence.Seed
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            AppDbContext context,
            IPasswordHasher passwordHasher,
            CancellationToken cancellationToken = default)
        {
            await context.Database.EnsureDeletedAsync(cancellationToken);
            await context.Database.MigrateAsync(cancellationToken);

            if (!context.Users.Any())
            {
                await context.Users.AddRangeAsync(BuildUsers(passwordHasher), cancellationToken);
            }

            if (!context.SoccerTeams.Any())
            {
                await context.SoccerTeams.AddRangeAsync(BuildTeams(), cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);

            if (!context.Matches.Any())
            {
                var teamsByCode = context.SoccerTeams.ToDictionary(x => x.Code, StringComparer.OrdinalIgnoreCase);
                await context.Matches.AddRangeAsync(BuildMatches(teamsByCode), cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);
        }

        private static IReadOnlyList<AppUser> BuildUsers(IPasswordHasher passwordHasher)
        {
            return
            [
                new AppUser(
                "WorldCup",
                "User",
                "user@test.com",
                "user",
                passwordHasher.HashPassword("User123"),
                UserRole.User),
                new AppUser(
                    "WorldCup",
                    "Admin",
                    "admin@test.com",
                    "admin",
                    passwordHasher.HashPassword("Admin123"),
                    UserRole.Admin)
            ];
        }

        private static IReadOnlyList<SoccerTeam> BuildTeams()
        {
            return
            [
                new SoccerTeam("Portugal", "pt", "K"),
                new SoccerTeam("Congo DR", "cd", "K"),
                new SoccerTeam("Uzbekistan", "uz", "K"),
                new SoccerTeam("Colombia", "co", "K"),

                new SoccerTeam("England", "gb-eng", "L"),
                new SoccerTeam("Croatia", "hr", "L"),
                new SoccerTeam("Ghana", "gh", "L"),
                new SoccerTeam("Panama", "pa", "L")
            ];
        }

        private static IReadOnlyList<Match> BuildMatches(IReadOnlyDictionary<string, SoccerTeam> teamsByCode)
        {
            return
            [
                new Match("K", teamsByCode["pt"], teamsByCode["cd"], new DateTime(2026, 6, 17, 18, 0, 0, DateTimeKind.Utc)),
                new Match("K", teamsByCode["uz"], teamsByCode["co"], new DateTime(2026, 6, 17, 21, 0, 0, DateTimeKind.Utc)),
                new Match("K", teamsByCode["pt"], teamsByCode["uz"], new DateTime(2026, 6, 23, 18, 0, 0, DateTimeKind.Utc)),
                new Match("K", teamsByCode["co"], teamsByCode["cd"], new DateTime(2026, 6, 23, 21, 0, 0, DateTimeKind.Utc)),
                new Match("K", teamsByCode["co"], teamsByCode["pt"], new DateTime(2026, 6, 27, 18, 0, 0, DateTimeKind.Utc)),
                new Match("K", teamsByCode["cd"], teamsByCode["uz"], new DateTime(2026, 6, 27, 21, 0, 0, DateTimeKind.Utc)),

                new Match("L", teamsByCode["gh"], teamsByCode["pa"], new DateTime(2026, 6, 17, 16, 0, 0, DateTimeKind.Utc)),
                new Match("L", teamsByCode["gb-eng"], teamsByCode["hr"], new DateTime(2026, 6, 17, 20, 0, 0, DateTimeKind.Utc)),
                new Match("L", teamsByCode["gb-eng"], teamsByCode["gh"], new DateTime(2026, 6, 23, 17, 0, 0, DateTimeKind.Utc)),
                new Match("L", teamsByCode["pa"], teamsByCode["hr"], new DateTime(2026, 6, 23, 20, 0, 0, DateTimeKind.Utc)),
                new Match("L", teamsByCode["pa"], teamsByCode["gb-eng"], new DateTime(2026, 6, 27, 17, 0, 0, DateTimeKind.Utc)),
                new Match("L", teamsByCode["hr"], teamsByCode["gh"], new DateTime(2026, 6, 27, 20, 0, 0, DateTimeKind.Utc))
            ];
        }
    }
}
