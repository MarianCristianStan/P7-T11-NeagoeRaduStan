using Library.Models;
using System.Collections.Generic;

namespace Library.Services.Interfaces
{
   public interface IPublisherService
   {
      List<Publisher> GetAllPublishers();
      Publisher GetPublisherById(int id);
      void AddPublisher(Publisher publisher);
      void UpdatePublisher(Publisher publisher);
      void DeletePublisher(int id);
   }
}