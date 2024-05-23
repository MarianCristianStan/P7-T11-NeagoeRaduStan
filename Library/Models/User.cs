using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;

namespace Library.Models
{
   public class User : IdentityUser
   {
      public string? FirstName { get; set; }
      public string? LastName { get; set; }
      public string? Gender { get; set; }
      public byte[]? ProfilePicture { get; set; }
      public ICollection<Review>? Reviews { get; set; }
      public ICollection<Request>? Requests { get; set; }
      public ICollection<ReservedBook>? ReservedBooks { get; set; }
   }
}