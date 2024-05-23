using Library.Models;
using System.Collections.Generic;

namespace Library.Services.Interfaces
{
   public interface IBookAuthorService
   {
      List<BookAuthor> GetAllBookAuthors();
      BookAuthor GetBookAuthorById(int id);
      void AddBookAuthor(BookAuthor bookAuthor);
      List<Author> GetAuthorsByBookISBN(string bookISBN);
      void UpdateBookAuthor(BookAuthor bookAuthor);
      void DeleteBookAuthor(int id);
      BookAuthor GetBookAuthorByBookAndAuthor(string isbn, int authorId);
      public List<BookAuthor> GetBookAuthorsByISBN(string bookISBN);
   }
}