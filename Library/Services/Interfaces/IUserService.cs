using Library.Models;

namespace Library.Services.Interfaces
{
	public interface IUserService
	{
		User GetCurrentUser();
		List<User> GetAllUsers();
		User GetUserById(string id);
		void UpdateUser(User user);
		void DeleteUser(string id);

		public  Task<bool> IsUserAdminAsync(User user);
	}
}
