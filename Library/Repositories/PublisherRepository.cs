using Library.Models;
using Library.Repositories.Interfaces;
using Library.Repositories;

namespace Library.Repositories
{
   public class PublisherRepository : RepositoryBase<Publisher>, IPublisherRepository
   {
      public PublisherRepository(LibraryContext libraryContext) : base(libraryContext)
      {
      }
   }
}


