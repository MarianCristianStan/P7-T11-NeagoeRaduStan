using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
   public class BookAuthorService : IBookAuthorService
   {
      private readonly IRepositoryWrapper _repositoryWrapper;
      

      public BookAuthorService(IRepositoryWrapper repositoryWrapper)
      {
         _repositoryWrapper = repositoryWrapper;
      }

      public List<BookAuthor> GetAllBookAuthors()
      {
         return _repositoryWrapper.BookAuthorRepository.FindAll().ToList();
      }
      public List<BookAuthor> GetBookAuthorsByISBN(string bookISBN)
      {
         return _repositoryWrapper.BookAuthorRepository
            .FindByCondition(ba => ba.BookISBN.Equals(bookISBN))
            .ToList();
      }

      public BookAuthor GetBookAuthorById(int id)
      {
         return _repositoryWrapper.BookAuthorRepository.FindByCondition(ba => ba.IdBookAuthor == id).FirstOrDefault();
      }

      public List<Author> GetAuthorsByBookISBN(string bookISBN)
      {
         return _repositoryWrapper.BookAuthorRepository
            .FindByCondition(ba => ba.BookISBN == bookISBN)
            .Include(ba => ba.Author)
            .Select(ba => ba.Author)
            .ToList();
      }
      public void AddBookAuthor(BookAuthor bookAuthor)
      {
         _repositoryWrapper.BookAuthorRepository.Create(bookAuthor);
         _repositoryWrapper.Save();
      }

      public void UpdateBookAuthor(BookAuthor bookAuthor)
      {
         _repositoryWrapper.BookAuthorRepository.Update(bookAuthor);
         _repositoryWrapper.Save();
      }

      public void DeleteBookAuthor(int id)
      {
         var bookAuthor = _repositoryWrapper.BookAuthorRepository.FindByCondition(ba => ba.IdBookAuthor == id).FirstOrDefault();
         if (bookAuthor != null)
         {
            _repositoryWrapper.BookAuthorRepository.Delete(bookAuthor);
            _repositoryWrapper.Save();
         }
         else
         {
            throw new ArgumentException($"BookAuthor with ID {id} not found.");
         }
      }

      public BookAuthor GetBookAuthorByBookAndAuthor(string isbn, int authorId)
      {
         return _repositoryWrapper.BookAuthorRepository
             .FindByCondition(ba => ba.BookISBN == isbn && ba.IdAuthor == authorId)
             .FirstOrDefault();
      }
   }
}
