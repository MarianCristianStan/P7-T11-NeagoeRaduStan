using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services;
using Library.Services.Interfaces;
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Legacy;

namespace UnitTests
{
    [TestFixture]
    public class ReviewServiceTests
    {
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private IReviewService _reviewService;
        private Mock<IReviewRepository> _mockReviewRepository;

        [SetUp]
        public void Setup()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockReviewRepository = new Mock<IReviewRepository>();

            _mockRepositoryWrapper.Setup(x => x.ReviewRepository).Returns(_mockReviewRepository.Object);
            _reviewService = new ReviewService(_mockRepositoryWrapper.Object);
        }

        [Test]
        public void GetAllReviews_ReturnsListOfReviews()
        {
            // Arrange
            var expectedReviews = new List<Review>
         {
            new Review { IdReview = 1, BookISBN = "1234567890" },
            new Review { IdReview = 2, BookISBN = "0987654321" },
         };

            _mockReviewRepository.Setup(x => x.FindAll()).Returns(expectedReviews.AsQueryable());

            // Act
            var result = _reviewService.GetAllReviews();

            // Assert
            Assert.That(result.Count, Is.EqualTo(expectedReviews.Count));
            CollectionAssert.AreEquivalent(expectedReviews, result);
        }

        [Test]
        public void GetReviewById_ReviewExists_ReturnsReview()
        {
            // Arrange
            var reviewId = 1;
            var review = new Review { IdReview = reviewId, BookISBN = "1234567890" };
            _mockReviewRepository.Setup(x => x.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<Func<Review, bool>>>()))
               .Returns(new List<Review> { review }.AsQueryable());

            // Act
            var result = _reviewService.GetReviewById(reviewId);

            // Assert
            Assert.That(result, Is.EqualTo(review));
        }

        [Test]
        public void GetReviewById_ReviewDoesNotExist_ReturnsNull()
        {
            // Arrange
            var reviewId = 1;
            _mockReviewRepository.Setup(x => x.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<Func<Review, bool>>>()))
               .Returns(new List<Review>().AsQueryable());

            // Act
            var result = _reviewService.GetReviewById(reviewId);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetReviewsByBookISBN_ReturnsListOfReviews()
        {
            // Arrange
            var isbn = "1234567890";
            var expectedReviews = new List<Review>
         {
            new Review { IdReview = 1, BookISBN = isbn },
            new Review { IdReview = 2, BookISBN = isbn },
         };

            _mockReviewRepository.Setup(x => x.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<Func<Review, bool>>>()))
               .Returns(expectedReviews.AsQueryable());

            // Act
            var result = _reviewService.GetReviewsByBookISBN(isbn);

            // Assert
            Assert.That(result.Count, Is.EqualTo(expectedReviews.Count));
            CollectionAssert.AreEquivalent(expectedReviews, result);
        }

        [Test]
        public void AddReview_AddsReview()
        {
            // Arrange
            var review = new Review { IdReview = 1, BookISBN = "1234567890" };

            // Act
            _reviewService.AddReview(review);

            // Assert
            _mockReviewRepository.Verify(x => x.Create(review), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Test]
        public void UpdateReview_UpdatesReview()
        {
            // Arrange
            var review = new Review { IdReview = 1, BookISBN = "1234567890" };

            // Act
            _reviewService.UpdateReview(review);

            // Assert
            _mockReviewRepository.Verify(x => x.Update(review), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Test]
        public void DeleteReview_ReviewExists_DeletesReview()
        {
            // Arrange
            var reviewId = 1;
            var review = new Review { IdReview = reviewId, BookISBN = "1234567890" };
            _mockReviewRepository.Setup(x => x.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<Func<Review, bool>>>()))
               .Returns(new List<Review> { review }.AsQueryable());

            // Act
            _reviewService.DeleteReview(reviewId);

            // Assert
            _mockReviewRepository.Verify(x => x.Delete(review), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Test]
        public void DeleteReview_ReviewDoesNotExist_ThrowsArgumentException()
        {
            // Arrange
            var reviewId = 1;
            _mockReviewRepository.Setup(x => x.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<Func<Review, bool>>>()))
               .Returns(new List<Review>().AsQueryable());

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _reviewService.DeleteReview(reviewId));
            Assert.That(ex.Message, Is.EqualTo($"Review with ID {reviewId} not found."));
        }
    }
}
