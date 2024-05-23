using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Services
{
   public class ReservedBookService : IReservedBookService
   {
      private readonly IRepositoryWrapper _repositoryWrapper;

      public ReservedBookService(IRepositoryWrapper repositoryWrapper)
      {
         _repositoryWrapper = repositoryWrapper;
      }

      public List<ReservedBook> GetAllReservedBooks()
      {
         return _repositoryWrapper.ReservedBookRepository.FindAll().ToList();
      }

      public ReservedBook GetReservedBookById(int id)
      {
         return _repositoryWrapper.ReservedBookRepository.FindByCondition(rb => rb.IdReservedBook == id).FirstOrDefault();
      }
		public async Task<ReservedBook> GetReservedBookByUserAndISBNAsync(string userId, string bookISBN)
		{
			return await _repositoryWrapper.ReservedBookRepository.FindByCondition(rb => rb.IdUser == userId && rb.BookISBN == bookISBN).FirstOrDefaultAsync();
		}
		public async Task<List<ReservedBook>> GetReservedBooksByUserAsync(string userId)
		{
			return await _repositoryWrapper.ReservedBookRepository
													 .FindByCondition(rb => rb.IdUser == userId)
													 .Include(rb => rb.Book)
													 .ToListAsync();
		}

		public async Task ClearReservedBooksByUserAsync(string userId)
		{
			var reservedBooks = await _repositoryWrapper.ReservedBookRepository
				.FindByCondition(rb => rb.IdUser == userId)
				.ToListAsync();
			foreach (var reservedBook in reservedBooks)
			{
				DeleteReservedBook(reservedBook.IdReservedBook);
			}
		}
   
	public void AddReservedBook(ReservedBook reservedBook)
      {
         _repositoryWrapper.ReservedBookRepository.Create(reservedBook);
         _repositoryWrapper.Save();
      }

      public void UpdateReservedBook(ReservedBook reservedBook)
      {
         _repositoryWrapper.ReservedBookRepository.Update(reservedBook);
         _repositoryWrapper.Save();
      }

      public void DeleteReservedBook(int id)
      {
         var reservedBook = _repositoryWrapper.ReservedBookRepository.FindByCondition(rb => rb.IdReservedBook == id).FirstOrDefault();
         if (reservedBook != null)
         {
            _repositoryWrapper.ReservedBookRepository.Delete(reservedBook);
            _repositoryWrapper.Save();
         }
         else
         {
            throw new ArgumentException($"ReservedBook with ID {id} not found.");
         }
      }
   }
}