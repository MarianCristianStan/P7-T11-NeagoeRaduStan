using Library.Models;

namespace Library.Services.Interfaces
{
	public interface IUserService
	{
		User GetCurrentUser();
		List<User> GetAllUsers();
		User GetUserById(int id);
		void AddUser(User user);
		void UpdateUser(User user);
		void DeleteUser(int id);

		public  Task<bool> IsUserAdminAsync(User user);
	}
}
