using Library.Models;
using Library.Repositories.Interfaces;

namespace Library.Repositories
{
	public class BookAuthorRepository : RepositoryBase<BookAuthor>, IBookAuthorRepository
	{
		public BookAuthorRepository(LibraryContext libraryContext) : base(libraryContext)
		{
		}
	}
}
