using Library.Models;
using Library.Repositories.Interfaces;

namespace Library.Repositories
{
	public class BookRepository : RepositoryBase<Book>, IBookRepository
	{
		public BookRepository(LibraryContext libraryContext)
			: base(libraryContext)
		{
		}

	}
}
