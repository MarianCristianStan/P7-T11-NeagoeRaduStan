using Library.Models;
using Library.Repositories.Interfaces;

namespace Library.Repositories
{
   public class ContactFormRepository : RepositoryBase<ContactForm>, IContactFormRepository
   {
      public ContactFormRepository(LibraryContext libraryContext)
         : base(libraryContext)
      {
      }
   }
}
