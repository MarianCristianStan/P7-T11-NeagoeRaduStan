using Library.Models;
using Library.Repositories.Interfaces;

namespace Library.Repositories
{
	public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
	{
		public AuthorRepository(LibraryContext libraryContext) : base(libraryContext)
		{
		}
	}
}
