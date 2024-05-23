using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Services
{
   public class BookService : IBookService
   {
      private readonly IRepositoryWrapper _repositoryWrapper;

      public BookService(IRepositoryWrapper repositoryWrapper)
      {
         _repositoryWrapper = repositoryWrapper;
      }

      public List<Book> GetAllBooks()
      {
         return _repositoryWrapper.BookRepository.FindAll().ToList();
      }

      public Book GetBookByISBN(string isbn)
      {
         return _repositoryWrapper.BookRepository.FindByCondition(b => b.ISBN == isbn).FirstOrDefault();
      }

      public void AddBook(Book book)
      {
         _repositoryWrapper.BookRepository.Create(book);
         _repositoryWrapper.Save();
      }

      public void UpdateBook(Book book)
      {
         _repositoryWrapper.BookRepository.Update(book);
         _repositoryWrapper.Save();
      }

      public void DeleteBook(string isbn)
      {
         var book = _repositoryWrapper.BookRepository.FindByCondition(b => b.ISBN == isbn).FirstOrDefault();
         if (book != null)
         {
            _repositoryWrapper.BookRepository.Delete(book);
            _repositoryWrapper.Save();
         }
         else
         {
            throw new ArgumentException($"Book with ISBN {isbn} not found.");
         }
      }
   }
}