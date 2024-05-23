using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
	public class Bookshelf
	{
		[Key]
		public int IdBookshelf { get; set; }
		public string? Category { get; set; }

		public ICollection<Book>? Books { get; set; }
	}
}
