using Library.Models;
using Library.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
	public class ReturnRequestRepository : RepositoryBase<ReturnRequest>, IReturnRequestRepository
	{
		public ReturnRequestRepository(LibraryContext repositoryContext) : base(repositoryContext)
		{
		}
	}
}


