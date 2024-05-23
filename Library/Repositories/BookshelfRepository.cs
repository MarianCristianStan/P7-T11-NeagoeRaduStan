using Library.Models;
using Library.Repositories.Interfaces;

namespace Library.Repositories
{
	public class BookshelfRepository : RepositoryBase<Bookshelf>, IBookshelfRepository
	{
		public BookshelfRepository(LibraryContext libraryContext) : base(libraryContext)
		{
		}
	}
}
