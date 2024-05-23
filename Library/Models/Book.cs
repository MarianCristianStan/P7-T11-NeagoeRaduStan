using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
	public class Book
	{
		[Key]
      public string ISBN { get; set; }
      public string Title { get; set; }
      public string Category { get; set; }
      public string ReleaseYear { get; set; }
      public ICollection<BookAuthor>? BookAuthors { get; set; }
		public ICollection<Request>? Requests { get; set; }
		public ICollection<Review>? Reviews { get; set; }
      public ICollection<ReservedBook>? ReservedBooks { get; set; }

      [ForeignKey("Bookshelf")]
      public int IdBookshelf { get; set; }
		public Bookshelf? Bookshelf { get; set; }

      [ForeignKey("Publisher")]
      public int IdPublisher { get; set; }
      public Publisher? Publisher { get; set; }
   }
}
