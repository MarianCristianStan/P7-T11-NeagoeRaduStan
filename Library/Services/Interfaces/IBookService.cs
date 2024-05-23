using Library.Models;
using System.Collections.Generic;

namespace Library.Services.Interfaces
{
   public interface IBookService
   {
      List<Book> GetAllBooks();
      Book GetBookByISBN(string isbn);
      void AddBook(Book book);
      void UpdateBook(Book book);
      void DeleteBook(string isbn);
   }
}