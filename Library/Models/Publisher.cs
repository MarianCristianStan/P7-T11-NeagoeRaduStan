using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
   public class Publisher
   {
      [Key]
      public int IdPublisher { get; set; }
      public string Name { get; set; }
      public ICollection<Book>? Books { get; set; }
   }
}
