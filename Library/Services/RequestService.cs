using Library.Models;
using Library.Repositories;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Services
{
   public class RequestService : IRequestService
   {
      private readonly IRepositoryWrapper _repositoryWrapper;

      public RequestService(IRepositoryWrapper repositoryWrapper)
      {
         _repositoryWrapper = repositoryWrapper;
      }

      public List<Request> GetAllRequests()
      {
         return _repositoryWrapper.RequestRepository.FindAll().ToList();
      }

      public Request GetRequestById(int id)
      {
         return _repositoryWrapper.RequestRepository.FindByCondition(r => r.IdRequest == id).FirstOrDefault();
      }

      public void AddRequest(Request request)
      {
         _repositoryWrapper.RequestRepository.Create(request);
         _repositoryWrapper.Save();
      }

      public void UpdateRequest(Request request)
      {
         _repositoryWrapper.RequestRepository.Update(request);
         _repositoryWrapper.Save();
      }

      public void DeleteRequest(int id)
      {
         var request = _repositoryWrapper.RequestRepository.FindByCondition(r => r.IdRequest == id).FirstOrDefault();
         if (request != null)
         {
            _repositoryWrapper.RequestRepository.Delete(request);
            _repositoryWrapper.Save();
         }
         else
         {
            throw new ArgumentException($"Request with ID {id} not found.");
         }
      }


      public Request GetRequestByUserAndBookISBN(string userId, string bookISBN)
      {
         return _repositoryWrapper.RequestRepository.FindByCondition(r => r.IdUser == userId && r.BookISBN == bookISBN).FirstOrDefault();
      }
   }

   
}