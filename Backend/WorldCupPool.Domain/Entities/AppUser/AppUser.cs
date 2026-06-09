using WorldCupPool.Domain.Common;
using WorldCupPool.Domain.Enums;

namespace WorldCupPool.Domain.Entities
{
    public sealed class AppUser : Entity<Guid>
    {
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string UserName { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string? RefreshToken { get; private set; }
        public UserRole Role { get; private set; } = UserRole.User;
        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
        public ICollection<Prediction> Predictions { get; private set; } = new List<Prediction>();


        public AppUser() { }

        public AppUser(string firstName, string lastName, string email, string username, string passwordHash, UserRole role)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = username;
            PasswordHash = passwordHash;
            Role = role;
            CreatedAtUtc = DateTime.UtcNow;
        }


        public void SetRefreshToken(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}
