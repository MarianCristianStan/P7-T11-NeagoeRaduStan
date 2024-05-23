using Library.Models;
using Library.Repositories.Interfaces;

namespace Library.Repositories
{
   public class ReservedBookRepository : RepositoryBase<ReservedBook>, IReservedBookRepository
   {
      public ReservedBookRepository(LibraryContext repositoryContext)
         : base(repositoryContext)
      {
      }
   }
}