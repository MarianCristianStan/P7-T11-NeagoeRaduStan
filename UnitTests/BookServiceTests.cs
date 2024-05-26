using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services;
using Library.Services.Interfaces;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    [TestFixture]
    public class BookServiceTests
    {
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private Mock<IBookRepository> _mockBookRepository;
        private IBookService _bookService;

        [SetUp]
        public void Setup()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockRepositoryWrapper.Setup(x => x.BookRepository).Returns(_mockBookRepository.Object);

            _bookService = new BookService(_mockRepositoryWrapper.Object);
        }

        [Test]
        public void GetAllBooks_ReturnsListOfBooks()
        {
            // Arrange
            var expectedBooks = new List<Book>
            {
                new Book { ISBN = "ddddd", Title = "Cum sa fii fericit" },

            };
            _mockBookRepository.Setup(x => x.FindAll()).Returns(expectedBooks.AsQueryable());

            // Act
            var result = _bookService.GetAllBooks();

            // Assert
            Assert.That(result.Count, Is.EqualTo(expectedBooks.Count));
            CollectionAssert.AreEquivalent(expectedBooks, result);
        }

        [Test]
        public void GetBookByISBN_BookExists_ReturnsBook()
        {
            // Arrange
            var isbn = "ddddd";
            var book = new Book { ISBN = isbn, Title = "Cum sa fii fericit" };
            _mockBookRepository.Setup(x => x.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<Func<Book, bool>>>()))
                .Returns(new List<Book> { book }.AsQueryable());

            // Act
            var result = _bookService.GetBookByISBN(isbn);

            // Assert
            Assert.That(result, Is.EqualTo(book));
        }

        [Test]
        public void GetBookByISBN_BookDoesNotExist_ReturnsNull()
        {
            // Arrange
            var isbn = "ddddd";
            _mockBookRepository.Setup(x => x.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<Func<Book, bool>>>()))
                .Returns(new List<Book>().AsQueryable());

            // Act
            var result = _bookService.GetBookByISBN(isbn);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void AddBook_AddsBook()
        {
            // Arrange
            var book = new Book { ISBN = "ddddd", Title = "Cum sa fii fericit" };

            // Act
            _bookService.AddBook(book);

            // Assert
            _mockBookRepository.Verify(x => x.Create(book), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Test]
        public void UpdateBook_UpdatesBook()
        {
            // Arrange
            var book = new Book { ISBN = "ddddd", Title = "Cum sa fii fericit" };

            // Act
            _bookService.UpdateBook(book);

            // Assert
            _mockBookRepository.Verify(x => x.Update(book), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Test]
        public void DeleteBook_BookExists_DeletesBook()
        {
            // Arrange
            var isbn = "ddddd";
            var book = new Book { ISBN = isbn, Title = "Cum sa fii fericit" };
            _mockBookRepository.Setup(x => x.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<Func<Book, bool>>>()))
                .Returns(new List<Book> { book }.AsQueryable());

            // Act
            _bookService.DeleteBook(isbn);

            // Assert
            _mockBookRepository.Verify(x => x.Delete(book), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Test]
        public void DeleteBook_BookDoesNotExist_ThrowsArgumentException()
        {
            // Arrange
            var isbn = "ddddd";
            _mockBookRepository.Setup(x => x.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<Func<Book, bool>>>()))
                .Returns(new List<Book>().AsQueryable());

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _bookService.DeleteBook(isbn));
            Assert.That(ex.Message, Is.EqualTo($"Book with ISBN {isbn} not found."));
        }
    }
}