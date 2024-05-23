using Library.Models;
using System.Collections.Generic;

namespace Library.Services.Interfaces
{
   public interface IReservedBookService
   {
      List<ReservedBook> GetAllReservedBooks();
      ReservedBook GetReservedBookById(int id);
      Task<ReservedBook> GetReservedBookByUserAndISBNAsync(string userId, string bookISBN);
      Task<List<ReservedBook>> GetReservedBooksByUserAsync(string userId);
       Task ClearReservedBooksByUserAsync(string userId);
		void AddReservedBook(ReservedBook reservedBook);
      void UpdateReservedBook(ReservedBook reservedBook);
      void DeleteReservedBook(int id);
   }
}