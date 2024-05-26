using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;
using Moq;
using System.Security.Claims;
using NUnit.Framework.Legacy;

namespace UnitTests
{
   [TestFixture]
   public class UserServiceTests
   {
      private Mock<IRepositoryWrapper> _mockRepositoryWrapper;
      private Mock<IHttpContextAccessor> _mockHttpContextAccessor;
      private Mock<UserManager<User>> _mockUserManager;
      private IUserService _userService;
      private Mock<IUserRepository> _mockUserRepository;


      [SetUp]
      public void Setup()
      {
         _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
         _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
         _mockUserManager = new Mock<UserManager<User>>(
             new Mock<IUserStore<User>>().Object,
             null, null, null, null, null, null, null, null);

         _mockUserRepository = new Mock<IUserRepository>();

         _mockRepositoryWrapper.Setup(x => x.UserRepository).Returns(_mockUserRepository.Object);
         _userService = new UserService(_mockRepositoryWrapper.Object, _mockHttpContextAccessor.Object, _mockUserManager.Object);
      }

      [Test]
      public void GetCurrentUser_Works()
      {
         // Arrange
         var userId = "66d78188-7c1d-44a4-8a5e-4de139be8145";
         var user = new User { Id = userId, FirstName = "test_Fname" };
         var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
         {
                new Claim(ClaimTypes.NameIdentifier, userId)
         }));
         _mockHttpContextAccessor.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);
         _mockUserManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
         _mockUserManager.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

         // Act
         var loggedUser = _userService.GetCurrentUser();

         // Assert
         Assert.That(loggedUser, Is.Not.Null);
         Assert.That(loggedUser.FirstName, Is.EqualTo("test_Fname"));
         Assert.That(loggedUser.Id, Is.EqualTo("66d78188-7c1d-44a4-8a5e-4de139be8145"));
      }

      [Test]
      public async Task IsUserAdminAsync_UserIsAdmin_ReturnsTrue()
      {
         // Arrange
         var user = new User { Id = "af3013ee-d157-4dd4-a34d-e237c55f3d41" };
         _mockUserManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Admin" });

         // Act
         var result = await _userService.IsUserAdminAsync(user);

         // Assert
         Assert.That(result, Is.True);
      }

      [Test]
      public async Task IsUserAdminAsync_UserIsNotAdmin_ReturnsFalse()
      {
         // Arrange
         var user = new User { Id = "66d78188-7c1d-44a4-8a5e-4de139be8145" };
         _mockUserManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });

         // Act
         var result = await _userService.IsUserAdminAsync(user);

         // Assert
         Assert.That(result, Is.False);
      }

      [Test]
      public async Task IsUserAdminAsync_UserIsNull_ReturnsFalse()
      {
         // Act
         var result = await _userService.IsUserAdminAsync(null);

         // Assert
         Assert.That(result, Is.False);
      }

      public void GetAllUsers_ReturnsListOfUsers()
      {
         // Arrange
         var expectedUsers = new List<User>
         {
            new User { Id = "06b27d4c-fed1-484a-a0bb-56c83edc303f" },
            new User { Id = "4627004e-2f67-4c50-8a25-937d3e311311" },
            new User { Id = "66d78188-7c1d-44a4-8a5e-4de139be8145" },
            new User { Id = "a061fdaa-f908-448d-9edd-6a9a66fa12e8" },
            new User { Id = "af3013ee-d157-4dd4-a34d-e237c55f3d41" },
         };

         _mockRepositoryWrapper.Setup(x => x.UserRepository.FindAll()).Returns(expectedUsers.AsQueryable());

         // Act
         var result = _userService.GetAllUsers();

         // Assert
         Assert.That(result.Count, Is.EqualTo(expectedUsers.Count));
         CollectionAssert.AreEquivalent(expectedUsers, result);
      }

      [Test]
      public void GetUserById_UserExists_ReturnsUser()
      {
         // Arrange
         var userId = "66d78188-7c1d-44a4-8a5e-4de139be8145";
         var user = new User { Id = userId };
         _mockRepositoryWrapper.Setup(x => x.UserRepository.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
            .Returns(new List<User> { user }.AsQueryable());

         // Act
         var result = _userService.GetUserById(userId);

         // Assert
         Assert.That(result, Is.EqualTo(user));
      }

      [Test]
      public void GetUserById_UserDoesNotExist_ReturnsNull()
      {
         // Arrange
         var userId = "userId";
         _mockRepositoryWrapper.Setup(x => x.UserRepository.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
             .Returns(new List<User>().AsQueryable());

         // Act
         var result = _userService.GetUserById(userId);

         // Assert
         Assert.That(result, Is.Null);
      }

      [Test]
      public void UpdateUser_UpdatesUser()
      {
         // Arrange
         var user = new User { Id = "66d78188-7c1d-44a4-8a5e-4de139be8145" };

         // Act
         _userService.UpdateUser(user);

         // Assert
         _mockUserRepository.Verify(x => x.Update(user), Times.Once);
         _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
      }

      [Test]
      public void DeleteUser_UserExists_DeletesUser()
      {
         // Arrange
         var userId = "66d78188-7c1d-44a4-8a5e-4de139be8145";
         var user = new User { Id = userId.ToString() };
         _mockUserRepository.Setup(x => x.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
            .Returns(new List<User> { user }.AsQueryable());

         // Act
         _userService.DeleteUser(userId);

         // Assert
         _mockUserRepository.Verify(x => x.Delete(user), Times.Once);
         _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
      }

      [Test]
      public void DeleteUser_UserDoesNotExist_ThrowsArgumentException()
      {
         // Arrange
         var userId = "userId";
         _mockRepositoryWrapper.Setup(x => x.UserRepository.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
             .Returns(new List<User>().AsQueryable());

         // Act & Assert
         var ex = Assert.Throws<ArgumentException>(() => _userService.DeleteUser(userId));
         Assert.That(ex.Message, Is.EqualTo($"User with ID {userId} not found."));
      }
   
   }
}
