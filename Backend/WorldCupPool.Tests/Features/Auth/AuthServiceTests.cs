using Moq;
using WorldCupPool.Application.Common;
using WorldCupPool.Application.Exceptions;
using WorldCupPool.Application.Features.Auth;
using WorldCupPool.Application.Features.Auth.DTOs.Requests;
using WorldCupPool.Domain.Entities;
using WorldCupPool.Domain.Enums;

namespace WorldCupPool.Tests.Auth;

public sealed class AuthServiceTests
{
    [Fact]
    public async Task SignUp_ShouldCreateUserWithRoleFromRequest()
    {
        var repository = new Mock<IAppUserRepository>();
        var jwt = new Mock<IJwtTokenGenerator>();
        var passwordHasher = new Mock<IPasswordHasher>();
        passwordHasher.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("hashed");
        jwt.Setup(x => x.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>())).Returns("token");

        AppUser? createdUser = null;
        repository
            .Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<CancellationToken>()))
            .Callback<AppUser, CancellationToken>((user, _) => createdUser = user)
            .Returns(Task.CompletedTask);

        var service = new AuthService(repository.Object, jwt.Object, passwordHasher.Object);

        var request = new RegisterRequestDto
        {
            FirstName = "Admin",
            LastName = "User",
            Email = "admin@test.com",
            Username = "admin",
            Password = "Admin123",
            Role = "ADMIN"
        };

        var response = await service.SignUp(request, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, response.UserId);
        Assert.NotNull(createdUser);
        Assert.Equal(UserRole.Admin, createdUser!.Role);
        repository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SignUp_ShouldThrowValidationException_WhenRoleIsInvalid()
    {
        var repository = new Mock<IAppUserRepository>();
        var jwt = new Mock<IJwtTokenGenerator>();
        var passwordHasher = new Mock<IPasswordHasher>();
        var service = new AuthService(repository.Object, jwt.Object, passwordHasher.Object);

        var request = new RegisterRequestDto
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@test.com",
            Username = "test",
            Password = "123456",
            Role = "SUPERADMIN"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => service.SignUp(request, CancellationToken.None));

        Assert.Equal("Role must be ADMIN or USER.", exception.Message);
        repository.Verify(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Login_ShouldThrowAuthenticationException_WhenPasswordIsInvalid()
    {
        var user = new AppUser("Test", "User", "test@test.com", "test", "hashed", UserRole.User);
        var repository = new Mock<IAppUserRepository>();
        var jwt = new Mock<IJwtTokenGenerator>();
        var passwordHasher = new Mock<IPasswordHasher>();

        repository
            .Setup(x => x.GetByUserNameAsync("test", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        passwordHasher
            .Setup(x => x.Verify("wrong-password", user.PasswordHash))
            .Returns(false);

        var service = new AuthService(repository.Object, jwt.Object, passwordHasher.Object);

        var exception = await Assert.ThrowsAsync<AuthenticationException>(() =>
            service.Login(new LoginRequestDto { Username = "test", Password = "wrong-password" }, CancellationToken.None));

        Assert.Equal("Invalid credentials.", exception.Message);
        repository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
