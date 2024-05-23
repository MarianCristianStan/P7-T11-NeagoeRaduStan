using Library.Models;
using System.Collections.Generic;

namespace Library.Services.Interfaces
{
   public interface IBookshelfService
   {
      List<Bookshelf> GetAllBookshelves();
      Bookshelf GetBookshelfById(int id);
      void AddBookshelf(Bookshelf bookshelf);
      void UpdateBookshelf(Bookshelf bookshelf);
      void DeleteBookshelf(int id);
   }
}