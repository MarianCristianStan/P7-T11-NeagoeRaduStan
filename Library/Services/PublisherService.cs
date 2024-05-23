using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Services
{
   public class PublisherService : IPublisherService
   {
      private readonly IRepositoryWrapper _repositoryWrapper;

      public PublisherService(IRepositoryWrapper repositoryWrapper)
      {
         _repositoryWrapper = repositoryWrapper;
      }

      public List<Publisher> GetAllPublishers()
      {
         return _repositoryWrapper.PublisherRepository.FindAll().ToList();
      }

      public Publisher GetPublisherById(int id)
      {
         return _repositoryWrapper.PublisherRepository.FindByCondition(p => p.IdPublisher == id).FirstOrDefault();
      }

      public void AddPublisher(Publisher publisher)
      {
         _repositoryWrapper.PublisherRepository.Create(publisher);
         _repositoryWrapper.Save();
      }

      public void UpdatePublisher(Publisher publisher)
      {
         _repositoryWrapper.PublisherRepository.Update(publisher);
         _repositoryWrapper.Save();
      }

      public void DeletePublisher(int id)
      {
         var publisher = _repositoryWrapper.PublisherRepository.FindByCondition(p => p.IdPublisher == id).FirstOrDefault();
         if (publisher != null)
         {
            _repositoryWrapper.PublisherRepository.Delete(publisher);
            _repositoryWrapper.Save();
         }
         else
         {
            throw new ArgumentException($"Publisher with ID {id} not found.");
         }
      }
   }
}