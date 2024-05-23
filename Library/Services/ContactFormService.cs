using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;

namespace Library.Services
{
   public class ContactFormService : IContactFormService
   {
      private readonly IRepositoryWrapper _repositoryWrapper;

      public ContactFormService(IRepositoryWrapper repositoryWrapper)
      {
         _repositoryWrapper = repositoryWrapper;
      }

      public List<ContactForm> GetAllContactForms()
      {
         return _repositoryWrapper.ContactFormRepository.FindAll().ToList();
      }

      public ContactForm GetContactFormById(int IdContactForm)
      {
         return _repositoryWrapper.ContactFormRepository.FindByCondition(c => c.IdContactForm == IdContactForm)
            .FirstOrDefault();
      }

      public void AddContactForm(ContactForm contactForm)
      {
         _repositoryWrapper.ContactFormRepository.Create(contactForm);
         _repositoryWrapper.Save();
      }
   }
}
