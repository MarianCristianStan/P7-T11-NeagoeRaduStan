using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Services
{
   public class ReviewService : IReviewService
   {
      private readonly IRepositoryWrapper _repositoryWrapper;

      public ReviewService(IRepositoryWrapper repositoryWrapper)
      {
         _repositoryWrapper = repositoryWrapper;
      }

      public List<Review> GetAllReviews()
      {
         return _repositoryWrapper.ReviewRepository.FindAll().ToList();
      }

      public Review GetReviewById(int id)
      {
         return _repositoryWrapper.ReviewRepository.FindByCondition(r => r.IdReview == id).FirstOrDefault();
      }

      public List<Review> GetReviewsByBookISBN(string isbn)
      {
         return _repositoryWrapper.ReviewRepository.FindByCondition(r => r.BookISBN == isbn).ToList();
      }

      public void AddReview(Review review)
      {
         _repositoryWrapper.ReviewRepository.Create(review);
         _repositoryWrapper.Save();
      }

      public void UpdateReview(Review review)
      {
         _repositoryWrapper.ReviewRepository.Update(review);
         _repositoryWrapper.Save();
      }

      public void DeleteReview(int id)
      {
         var review = _repositoryWrapper.ReviewRepository.FindByCondition(r => r.IdReview == id).FirstOrDefault();
         if (review != null)
         {
            _repositoryWrapper.ReviewRepository.Delete(review);
            _repositoryWrapper.Save();
         }
         else
         {
            throw new ArgumentException($"Review with ID {id} not found.");
         }
      }
   }
}