using Library.Areas.Identity.Pages.Account;
using Library.Models;
using Library.Services;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace UnitTests
{
   [TestFixture]
   public class AuthServiceTests
   {
      private Mock<UserManager<User>> _mockUserManager;
      private Mock<IUserStore<User>> _mockUserStore;
      private Mock<IUserEmailStore<User>> _mockEmailStore;
      private Mock<SignInManager<User>> _mockSignInManager;
      private Mock<IOptions<IdentityOptions>> _mockIdentityOptions;
      private IAuthService _authService;

      [SetUp]
      public void Setup()
      {
         _mockUserStore = new Mock<IUserStore<User>>();
         _mockEmailStore = _mockUserStore.As<IUserEmailStore<User>>();

         _mockUserManager = new Mock<UserManager<User>>(
            _mockUserStore.Object,
            null, null, null, null, null, null, null, null);

         _mockUserManager.Setup(um => um.SupportsUserEmail).Returns(true);

         _mockIdentityOptions = new Mock<IOptions<IdentityOptions>>();
         var identityOptions = new IdentityOptions();
         _mockIdentityOptions.Setup(opt => opt.Value).Returns(identityOptions);

         _mockSignInManager = new Mock<SignInManager<User>>(
            _mockUserManager.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            null, null, null, null);

         _authService = new AuthService(_mockUserManager.Object, _mockUserStore.Object, _mockSignInManager.Object);
      }

      [Test]
      public async Task RegisterUserAsync_ValidInput_ReturnsSuccess()
      {
         // Arrange
         var inputModel = new RegisterModel.InputModel
         {
            Username = "testuser",
            Email = "testuser@example.com",
            Password = "Password123!"
         };

         var user = new User { UserName = inputModel.Username, Email = inputModel.Email };
         _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

         // Act
         var result = await _authService.RegisterUserAsync(inputModel);

         // Assert
         Assert.That(result.Succeeded, Is.True);
         _mockUserStore.Verify(x => x.SetUserNameAsync(It.Is<User>(u => u.UserName == inputModel.Username), inputModel.Username, CancellationToken.None), Times.Once);
         _mockEmailStore.Verify(x => x.SetEmailAsync(It.Is<User>(u => u.Email == inputModel.Email), inputModel.Email, CancellationToken.None), Times.Once);
      }
      [Test]
      public async Task HandleUserRegistrationAsync_ConfirmedAccountNotRequired_ReturnsLocalRedirectResult()
      {
         // Arrange
         var email = "testuser@example.com";
         var returnUrl = "/home";
         var user = new User { Email = email };
         _mockUserManager.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync(user);
         _mockIdentityOptions.Object.Value.SignIn.RequireConfirmedAccount = false;

         // Act
         var result = await _authService.HandleUserRegistrationAsync(email, returnUrl);

         // Assert
         Assert.That(result, Is.TypeOf<LocalRedirectResult>());
         var localRedirectResult = result as LocalRedirectResult;
         Assert.That(localRedirectResult.Url, Is.EqualTo(returnUrl));
         _mockSignInManager.Verify(x => x.SignInAsync(user, false, null), Times.Once);
      }

      [Test]
      public async Task GetExternalAuthenticationSchemesAsync_ReturnsSchemes()
      {
         // Arrange
         var schemes = new List<AuthenticationScheme>
            {
                new AuthenticationScheme("Scheme1", "Scheme 1", typeof(IAuthenticationHandler)),
                new AuthenticationScheme("Scheme2", "Scheme 2", typeof(IAuthenticationHandler))
            };

         _mockSignInManager.Setup(x => x.GetExternalAuthenticationSchemesAsync()).ReturnsAsync(schemes);

         // Act
         var result = await _authService.GetExternalAuthenticationSchemesAsync();

         // Assert
         Assert.That(result, Is.EquivalentTo(schemes));
      }

      [Test]
      public async Task HandleUserLoginAsync_ValidCredentials_ReturnsLocalRedirectResult()
      {
         // Arrange
         var inputModel = new LoginModel.InputModel
         {
            Username = "testuser",
            Password = "Password123!",
            RememberMe = true
         };
         var returnUrl = "/home";
         _mockSignInManager.Setup(x => x.PasswordSignInAsync(inputModel.Username, inputModel.Password, inputModel.RememberMe, false))
             .ReturnsAsync(SignInResult.Success);

         // Act
         var result = await _authService.HandleUserLoginAsync(inputModel, returnUrl);

         // Assert
         Assert.That(result, Is.TypeOf<LocalRedirectResult>());
         var localRedirectResult = result as LocalRedirectResult;
         Assert.That(localRedirectResult.Url, Is.EqualTo(returnUrl));
      }

      [Test]
      public async Task HandleUserLoginAsync_RequiresTwoFactor_ReturnsRedirectToPageResult()
      {
         // Arrange
         var inputModel = new LoginModel.InputModel
         {
            Username = "testuser",
            Password = "Password123!",
            RememberMe = true
         };
         var returnUrl = "/home";
         _mockSignInManager.Setup(x => x.PasswordSignInAsync(inputModel.Username, inputModel.Password, inputModel.RememberMe, false))
             .ReturnsAsync(SignInResult.TwoFactorRequired);

         // Act
         var result = await _authService.HandleUserLoginAsync(inputModel, returnUrl);

         // Assert
         Assert.That(result, Is.TypeOf<RedirectToPageResult>());
         var redirectResult = result as RedirectToPageResult;
         Assert.That(redirectResult.PageName, Is.EqualTo("./LoginWith2fa"));
         Assert.That(redirectResult.RouteValues["ReturnUrl"], Is.EqualTo(returnUrl));
         Assert.That(redirectResult.RouteValues["RememberMe"], Is.EqualTo(inputModel.RememberMe));
      }

      [Test]
      public async Task HandleUserLoginAsync_IsLockedOut_ReturnsRedirectToPageResult()
      {
         // Arrange
         var inputModel = new LoginModel.InputModel
         {
            Username = "testuser",
            Password = "Password123!",
            RememberMe = true
         };
         var returnUrl = "/home";
         _mockSignInManager.Setup(x => x.PasswordSignInAsync(inputModel.Username, inputModel.Password, inputModel.RememberMe, false))
             .ReturnsAsync(SignInResult.LockedOut);

         // Act
         var result = await _authService.HandleUserLoginAsync(inputModel, returnUrl);

         // Assert
         Assert.That(result, Is.TypeOf<RedirectToPageResult>());
         var redirectResult = result as RedirectToPageResult;
         Assert.That(redirectResult.PageName, Is.EqualTo("./Lockout"));
      }

      [Test]
      public async Task HandleUserLoginAsync_InvalidCredentials_ReturnsNull()
      {
         // Arrange
         var inputModel = new LoginModel.InputModel
         {
            Username = "testuser",
            Password = "Password123!",
            RememberMe = true
         };
         var returnUrl = "/home";
         _mockSignInManager.Setup(x => x.PasswordSignInAsync(inputModel.Username, inputModel.Password, inputModel.RememberMe, false))
             .ReturnsAsync(SignInResult.Failed);

         // Act
         var result = await _authService.HandleUserLoginAsync(inputModel, returnUrl);

         // Assert
         Assert.That(result, Is.Null);
      }
   }
}
