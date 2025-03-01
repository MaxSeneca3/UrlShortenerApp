using Moq;
using AutoMapper;
using BusinessLogic.Dtos;
using BusinessLogic.Services;
using BusinessLogic.Interfaces;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;
using BusinessLogic.Options;
using FluentResults;

namespace Testing.ServiceTests
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly Mock<TokenService> _tokenServiceMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userManagerMock = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            _tokenServiceMock = new Mock<TokenService>(Mock.Of<IOptions<JwtOptions>>());

            _authService = new AuthService(
                _userManagerMock.Object,
                _roleManagerMock.Object,
                _tokenServiceMock.Object
            );
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnSuccess_WhenUserIsRegisteredSuccessfully()
        {
            var registerDto = new RegisterDto
            {
                Username = "newuser",
                Email = "newuser@example.com",
                Password = "SecurePass123!",
                Role = "User"
            };

            var newUser = new AppUser { UserName = registerDto.Username, Email = registerDto.Email };

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), registerDto.Password))
                .ReturnsAsync(IdentityResult.Success);
            _roleManagerMock.Setup(x => x.RoleExistsAsync(registerDto.Role))
                .ReturnsAsync(false); // Role does not exist
            _roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), registerDto.Role))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _authService.RegisterAsync(registerDto);

            result.Succeeded.ShouldBeTrue();
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnFail_WhenRoleCreationFails()
        {
            var registerDto = new RegisterDto
            {
                Username = "newuser",
                Email = "newuser@example.com",
                Password = "SecurePass123!",
                Role = "User"
            };

            var newUser = new AppUser { UserName = registerDto.Username, Email = registerDto.Email };

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), registerDto.Password))
                .ReturnsAsync(IdentityResult.Success);
            _roleManagerMock.Setup(x => x.RoleExistsAsync(registerDto.Role))
                .ReturnsAsync(false); // Role does not exist
            _roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Role creation failed" }));

            var result = await _authService.RegisterAsync(registerDto);

            result.Succeeded.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.Description == "Role creation failed");
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnNull_WhenUserNotFound()
        {
            var loginDto = new LoginDto { Username = "nonexistentuser", Password = "Password123!" };
            _userManagerMock.Setup(x => x.FindByNameAsync(loginDto.Username))
                .ReturnsAsync((AppUser)null); // User not found

            var result = await _authService.AuthenticateAsync(loginDto);

            result.ShouldBeNull();
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnNull_WhenPasswordIsIncorrect()
        {
            var loginDto = new LoginDto { Username = "existinguser", Password = "WrongPassword" };
            var user = new AppUser { UserName = loginDto.Username };
            
            _userManagerMock.Setup(x => x.FindByNameAsync(loginDto.Username))
                .ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, loginDto.Password))
                .ReturnsAsync(false); // Incorrect password

            var result = await _authService.AuthenticateAsync(loginDto);

            result.ShouldBeNull();
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            var loginDto = new LoginDto { Username = "existinguser", Password = "Password123!" };
            var user = new AppUser { UserName = loginDto.Username };
            var roles = new List<string> { "User" };
            var token = "ValidJwtToken";

            _userManagerMock.Setup(x => x.FindByNameAsync(loginDto.Username))
                .ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, loginDto.Password))
                .ReturnsAsync(true); // Correct password
            _userManagerMock.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(roles);
            _tokenServiceMock.Setup(x => x.GenerateJwtToken(user, roles))
                .Returns(token);

            var result = await _authService.AuthenticateAsync(loginDto);

            result.ShouldNotBeNull();
            result.ShouldBe(token);
        }
    }
}
