using Library.Models;
using Library.Repositories.Interfaces;

namespace Library.Repositories
{
	public class RequestRepository : RepositoryBase<Request>, IRequestRepository
	{
		public RequestRepository(LibraryContext libraryContext) : base(libraryContext)
		{
		}
	}
}
