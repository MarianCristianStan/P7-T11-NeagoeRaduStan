using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Services
{
   public class AuthorService : IAuthorService
   {
      private readonly IRepositoryWrapper _repositoryWrapper;

      public AuthorService(IRepositoryWrapper repositoryWrapper)
      {
         _repositoryWrapper = repositoryWrapper;
      }

      public List<Author> GetAllAuthors()
      {
         return _repositoryWrapper.AuthorRepository.FindAll().ToList();
      }

      public Author GetAuthorById(int id)
      {
         return _repositoryWrapper.AuthorRepository.FindByCondition(a => a.IdAuthor == id).FirstOrDefault();
      }

      public Author GetAuthorByName(string name)
      {
         name = name.ToLower();
         return _repositoryWrapper.AuthorRepository.FindByCondition(a => a.Name.ToLower() == name).FirstOrDefault();
      }

      public void AddAuthor(Author author)
      {
         _repositoryWrapper.AuthorRepository.Create(author);
         _repositoryWrapper.Save();
      }

      public void UpdateAuthor(Author author)
      {
         _repositoryWrapper.AuthorRepository.Update(author);
         _repositoryWrapper.Save();
      }

      public void DeleteAuthor(int id)
      {
         var author = _repositoryWrapper.AuthorRepository.FindByCondition(a => a.IdAuthor == id).FirstOrDefault();
         if (author != null)
         {
            _repositoryWrapper.AuthorRepository.Delete(author);
            _repositoryWrapper.Save();
         }
         else
         {
            throw new ArgumentException($"Author with ID {id} not found.");
         }
      }
   }
}