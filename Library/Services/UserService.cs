using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Library.Services
{
	public class UserService : IUserService
	{
		private readonly IRepositoryWrapper _repositoryWrapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly UserManager<User> _userManager;
		public UserService(IRepositoryWrapper repositoryWrapper, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
		{
			_repositoryWrapper = repositoryWrapper;
			_httpContextAccessor = httpContextAccessor;
			_userManager = userManager;
		}

		public User GetCurrentUser()
		{
			var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
				

			// Ensure userId is not null
			if (string.IsNullOrEmpty(userId))
			{
				return null;
			}

			return (User)_userManager.FindByIdAsync(userId).Result;
		}
		public async Task<bool> IsUserAdminAsync(User user)
		{
			if (user == null) return false;
			var roles = await _userManager.GetRolesAsync(user);
			return roles.Contains("Admin");
		}

		public List<User> GetAllUsers()
		{
			return (List < User >) _repositoryWrapper.UserRepository.FindAll();
		}

		public User GetUserById(int id)
		{
			return (User)_repositoryWrapper.UserRepository.FindByCondition(u => u.Id.Equals(id)).FirstOrDefault();
		}

		public void AddUser(User user)
		{
			_repositoryWrapper.UserRepository.Create(user);
			_repositoryWrapper.Save();
		}

		public void UpdateUser(User user)
		{
			_repositoryWrapper.UserRepository.Update(user);
			_repositoryWrapper.Save();
		}

		public void DeleteUser(int id)
		{
			var user = _repositoryWrapper.UserRepository.FindByCondition(u => u.Id.Equals('1')).FirstOrDefault();
			if (user != null)
			{
				_repositoryWrapper.UserRepository.Delete(user);
				_repositoryWrapper.Save();
			}
			else
			{
				throw new ArgumentException($"User with ID {id} not found.");
			}
		}

		 
	}
}
