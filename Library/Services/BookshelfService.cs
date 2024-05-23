using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Services
{
   public class BookshelfService : IBookshelfService
   {
      private readonly IRepositoryWrapper _repositoryWrapper;

      public BookshelfService(IRepositoryWrapper repositoryWrapper)
      {
         _repositoryWrapper = repositoryWrapper;
      }

      public List<Bookshelf> GetAllBookshelves()
      {
         return _repositoryWrapper.BookshelfRepository.FindAll().ToList();
      }

      public Bookshelf GetBookshelfById(int id)
      {
         return _repositoryWrapper.BookshelfRepository.FindByCondition(b => b.IdBookshelf == id).FirstOrDefault();
      }

      public void AddBookshelf(Bookshelf bookshelf)
      {
         _repositoryWrapper.BookshelfRepository.Create(bookshelf);
         _repositoryWrapper.Save();
      }

      public void UpdateBookshelf(Bookshelf bookshelf)
      {
         _repositoryWrapper.BookshelfRepository.Update(bookshelf);
         _repositoryWrapper.Save();
      }

      public void DeleteBookshelf(int id)
      {
         var bookshelf = _repositoryWrapper.BookshelfRepository.FindByCondition(b => b.IdBookshelf == id).FirstOrDefault();
         if (bookshelf != null)
         {
            _repositoryWrapper.BookshelfRepository.Delete(bookshelf);
            _repositoryWrapper.Save();
         }
         else
         {
            throw new ArgumentException($"Bookshelf with ID {id} not found.");
         }
      }
   }
}