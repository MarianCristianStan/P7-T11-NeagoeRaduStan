using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.Models
{
	public class LibraryContext : IdentityDbContext<User>
   {
		public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
		{

		}	
      protected override void OnModelCreating(ModelBuilder builder)
      {
         base.OnModelCreating(builder);
      }
      public DbSet<Author> Authors { get; set; }
		public DbSet<Publisher> Publisher { get; set; }
		public DbSet<Bookshelf> Bookshelves { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<Request> Requests { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<BookAuthor> BooksAuthors { get; set; }
      public DbSet<ReservedBook> ReservedBooks { get; set; }
		public DbSet<ContactForm> ContactForm { get; set; }
		public DbSet<ReturnRequest> ReturnRequests {get; set; }

   }
}
