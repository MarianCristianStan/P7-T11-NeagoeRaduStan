using Library.Models;
using System.Collections.Generic;

namespace Library.Services.Interfaces
{
   public interface IRequestService
   {
      List<Request> GetAllRequests();
      Request GetRequestById(int id);
      void AddRequest(Request request);
      void UpdateRequest(Request request);
      void DeleteRequest(int id);
      Request GetRequestByUserAndBookISBN(string userId, string bookISBN);
   }
}