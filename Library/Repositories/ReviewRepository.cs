using Library.Models;
using Library.Repositories.Interfaces;

namespace Library.Repositories
{
	public class ReviewRepository : RepositoryBase<Review>, IReviewRepository
	{
		public ReviewRepository(LibraryContext libraryContext) : base(libraryContext)
		{
		}
	}
}
