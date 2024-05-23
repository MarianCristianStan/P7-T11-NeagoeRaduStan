using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
	public class Author
	{
		[Key]
		public int IdAuthor { get; set; }
		public string? Name { get; set; }
		public ICollection<BookAuthor>? AuthorBooks { get; set; }
	}
}
