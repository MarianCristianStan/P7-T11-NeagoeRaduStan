using Library.Models;
using Library.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Library.Repositories
{
	public class UserRepository : RepositoryBase<User>, IUserRepository
	{
		public UserRepository(LibraryContext libraryContext) : base(libraryContext)
		{
		}
	}
}
