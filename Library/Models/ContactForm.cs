using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
   public class ContactForm
   {
      [Key]
      public int IdContactForm { get; set;}
      public string? Name { get; set; }
      
      public string? Phone { get; set; }

      public string? Subject { get; set; }

      public string? Message { get; set; }
   }
}
