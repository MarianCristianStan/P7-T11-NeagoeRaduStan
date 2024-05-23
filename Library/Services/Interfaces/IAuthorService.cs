using Library.Models;
using System.Collections.Generic;

namespace Library.Services.Interfaces
{
   public interface IAuthorService
   {
      List<Author> GetAllAuthors();
      Author GetAuthorById(int id);
      Author GetAuthorByName(string name);
      void AddAuthor(Author author);
      void UpdateAuthor(Author author);
      void DeleteAuthor(int id);
   }
}