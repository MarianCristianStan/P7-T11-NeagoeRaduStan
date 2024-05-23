using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
	public class BookAuthor
	{
		[Key]
		public int IdBookAuthor { get; set; }
		[ForeignKey("Book")]
		public string BookISBN { get; set; }
		public  Book? Book { get; set; }

		[ForeignKey("Author")]
		public int IdAuthor { get; set; }
		public  Author? Author { get; set; }
	}
}
