using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using Scaffolding.Auth;
using Scaffolding.Auth.Entities;
using Scaffolding.Auth.Infrastructure;
using Scaffolding.Auth.Model;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Scaffolding.Auth.Tests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private AuthenticationService _authenticationService;
        private Mock<IHttpContextAccessor> _contextAccessorMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private Mock<RoleManager<ApplicationRole>> _roleManagerMock;

        [SetUp]
        public void Setup()
        {
            _contextAccessorMock = new Mock<IHttpContextAccessor>();
            _contextAccessorMock.Setup(x => x.HttpContext).Returns(Mock.Of<HttpContext>());

            _userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(_userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null);
            _roleManagerMock = new Mock<RoleManager<ApplicationRole>>(Mock.Of<IRoleStore<ApplicationRole>>(), null, null, null, null);

            _authenticationService = new AuthenticationService(
                _contextAccessorMock.Object,
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _roleManagerMock.Object
            );
        }

        [Test]
        public async Task GetCurrentUserAsync_WithValidClaim_ReturnsCurrentUser()
        {
            // Arrange
            var expectedUserId = Guid.NewGuid();
            var claim = new Claim("sub", expectedUserId.ToString());
            var claimsIdentity = new ClaimsIdentity(new[] { claim });
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            _contextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(claimsPrincipal);

            var expectedUser = new ApplicationUser { Id = expectedUserId };
            _userManagerMock.Setup(x => x.FindByIdAsync(expectedUserId.ToString())).ReturnsAsync(expectedUser);

            // Act
            var currentUser = await _authenticationService.GetCurrentUserAsync();

            // Assert
           currentUser.Should().BeEquivalentTo(expectedUser);
        }

        [Test]
        public async Task GetCurrentUserAsync_WithNoClaim_ReturnsNull()
        {
            // Arrange
            _contextAccessorMock.SetupGet(x => x.HttpContext.User).Returns((ClaimsPrincipal)null);

            // Act
            var currentUser = await _authenticationService.GetCurrentUserAsync();

            // Assert
           currentUser.Should().BeNull();
        }

        
        [Test]
        public void IsAuthenticate_WithAuthenticatedUser_ReturnsTrue()
        {
            // Arrange
            IIdentity identity = new GenericIdentity("john", "Token");
            _contextAccessorMock.SetupGet(x => x.HttpContext.User.Identity).Returns(identity);

            // Act
            var isAuthenticated = _authenticationService.IsAuthenticate();

            // Assert
           isAuthenticated.Should().BeTrue();
        }

        [Test]
        public void IsAuthenticate_WithNoAuthenticatedUser_ReturnsFalse()
        {
            // Arrange
            _contextAccessorMock.SetupGet(x => x.HttpContext.User.Identity).Returns((IIdentity)null);

            // Act
            var isAuthenticated = _authenticationService.IsAuthenticate();

            // Assert
            isAuthenticated.Should().BeFalse();
        }

        // create unit test for GetUserRole
        [Test]
        public void GetUserRole_WithValidClaim_ReturnsCurrentUserRole()
        {
            // Arrange
            var expectedUserId = Guid.NewGuid();
            var claim = new Claim("sub", expectedUserId.ToString());
            var role = new Claim("role", "Admin");
            var claimsIdentity = new ClaimsIdentity(new[] { claim, role });
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            _contextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(claimsPrincipal);
            var expectedUser = new ApplicationUser { Id = expectedUserId };
            _userManagerMock.Setup(x => x.FindByIdAsync(expectedUserId.ToString())).ReturnsAsync(expectedUser);
            var expectedRole = new ApplicationRole { Name = "Admin" };
            _roleManagerMock.Setup(x => x.FindByIdAsync(expectedRole.Name)).ReturnsAsync(expectedRole);
            
            // Act
            var currentUserRole = _authenticationService.GetUserRole();

            // Assert
            currentUserRole.Should().BeEquivalentTo(expectedRole.Name);
        }

        // create unit test for GetUserRole return string.empty
        [Test]
        public void GetUserRole_WithNoClaim_ReturnsNull()
        {
            // Arrange
            _contextAccessorMock.SetupGet(x => x.HttpContext.User).Returns((ClaimsPrincipal)null);
            // Act
            var currentUserRole = _authenticationService.GetUserRole();
            // Assert
            currentUserRole.Should().BeNull();
        }

        // create unit test for IsAdmin return true
        [Test]
        public void IsAdmin_WithValidClaim_ReturnsTrue()
        {
            // Arrange
            var expectedUserId = Guid.NewGuid();
            var claim = new Claim("sub", expectedUserId.ToString());
            var claimsIdentity = new ClaimsIdentity(new[] { claim });
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            _contextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(claimsPrincipal);
            var expectedUser = new ApplicationUser { Id = expectedUserId };
            _userManagerMock.Setup(x => x.FindByIdAsync(expectedUserId.ToString())).ReturnsAsync(expectedUser);
            var expectedRole = new ApplicationRole { Name = "Admin" };
            _roleManagerMock.Setup(x => x.FindByIdAsync(expectedRole.Name)).ReturnsAsync(expectedRole);
            
            // Act
            var isAdmin = _authenticationService.IsSuperAdmin();
            
            // Assert
            isAdmin.Should().BeTrue();
        }

        // create unit test for IsAdmin return false
        [Test]
        public void IsAdmin_WithNoClaim_ReturnsFalse()
        {
            // Arrange
            _contextAccessorMock.SetupGet(x => x.HttpContext.User).Returns((ClaimsPrincipal)null);

            // Act
            var isAdmin = _authenticationService.IsSuperAdmin();

            // Assert
            isAdmin.Should().BeFalse();
        }

        [Test]
        public async Task RegisterAsync_WithValidModel_ReturnsSuccessfulResult()
        {
            // Arrange
            var model = new RegisterModel
            {
                Username = "john",
                Email = "john@example.com",
                CellPhone = "1234567890",
                Password = "Password123"
            };
            var emailConfirmed = true;
            var smsConfirmed = true;

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync("emailToken");

            _userManagerMock.Setup(x => x.GenerateChangePhoneNumberTokenAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync("smsToken");

            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), MembershipConstant.User))
                .ReturnsAsync(IdentityResult.Success);

            _signInManagerMock.Setup(x => x.SignInAsync(It.IsAny<ApplicationUser>(), false, null))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _authenticationService.RegisterAsync(model, emailConfirmed, smsConfirmed);

            // Assert
            result.Should().NotBeNull();
            result.Result.Succeeded.Should().BeTrue();
            result.GeneratedToken.Should().Be("emailToken");
            result.GenerateSMSToken.Should().Be("smsToken");
            

            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()), Times.Once);
            _userManagerMock.Verify(x => x.GenerateChangePhoneNumberTokenAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), MembershipConstant.User), Times.Once);
            _signInManagerMock.Verify(x => x.SignInAsync(It.IsAny<ApplicationUser>(), false, null), Times.Once);
        }

        [Test]
        public async Task LoginAsync_WithValidCredentials_ReturnsClaimsIdentity()
        {
            // Arrange
            var model = new LoginModel
            {
                Username = "john",
                Password = "Password123"
            };

            var user = new ApplicationUser { UserName = model.Username };
            var claims = new[]
            {
                new Claim("role", "admin"),
                new Claim("email", "john@example.com")
            };
            var identity = new ClaimsIdentity(new GenericIdentity(model.Username, "Token"), claims);

            _signInManagerMock.Setup(x => x.PasswordSignInAsync(model.Username, model.Password, false, false))
                .ReturnsAsync(SignInResult.Success);
            _userManagerMock.Setup(x => x.FindByNameAsync(model.Username))
                .ReturnsAsync(user);
            _userManagerMock.Setup(x => x.GetClaimsAsync(user))
                .ReturnsAsync(claims);

            // Act
            var result = await _authenticationService.LoginAsync(model);

            // Assert
            result.Should().BeEquivalentTo(identity);

            _signInManagerMock.Verify(x => x.PasswordSignInAsync(model.Username, model.Password, false, false), Times.Once);
            _userManagerMock.Verify(x => x.FindByNameAsync(model.Username), Times.Once);
            _userManagerMock.Verify(x => x.GetClaimsAsync(user), Times.Once);
        }

        [Test]
        public async Task LoginAsync_WithInvalidCredentials_ReturnsNull()
        {
            // Arrange
            var model = new LoginModel
            {
                Username = "john",
                Password = "InvalidPassword"
            };

            _signInManagerMock.Setup(x => x.PasswordSignInAsync(model.Username, model.Password, false, false))
                .ReturnsAsync(SignInResult.Failed);

            // Act
            var result = await _authenticationService.LoginAsync(model);

            // Assert
            result.Should().BeNull();

            _signInManagerMock.Verify(x => x.PasswordSignInAsync(model.Username, model.Password, false, false), Times.Once);
            _userManagerMock.Verify(x => x.FindByNameAsync(model.Username), Times.Never);
            _userManagerMock.Verify(x => x.GetClaimsAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }


    }
}
