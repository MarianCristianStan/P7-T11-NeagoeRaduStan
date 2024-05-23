using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
	public class ReturnRequest
	{
		[Key]
		public int IdRequest { get; set; }
		public DateTime RequestDate { get; set; }
		public string? Description { get; set; }
		public bool IsApproved { get; set; }

		[ForeignKey("User")]
		public string IdUser { get; set; }
		public User? User { get; set; }

		[ForeignKey("Book")]
		public string BookISBN { get; set; }
		public Book? Book { get; set; }
	}
}