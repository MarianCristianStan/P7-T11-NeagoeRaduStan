using Library.Models;

namespace Library.Services.Interfaces
{
   public interface IContactFormService
   {
      List<ContactForm> GetAllContactForms();
      ContactForm GetContactFormById(int IdContactForm);
      void AddContactForm(ContactForm contactForm);

   }
}
