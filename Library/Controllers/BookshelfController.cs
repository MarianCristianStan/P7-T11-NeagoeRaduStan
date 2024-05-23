using Library.Models;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace Library.Controllers
{
   public class BookshelfController : Controller
   {
      private readonly IBookService _bookService;
      private readonly IBookshelfService _bookshelfService;
      private readonly IPublisherService _publisherService;
      private readonly IAuthorService _authorService;
      private readonly IBookAuthorService _bookAuthorService;

      public BookshelfController(
          IBookService bookService,
          IBookshelfService bookshelfService,
          IPublisherService publisherService,
          IAuthorService authorService,
          IBookAuthorService bookAuthorService)
      {
         _bookService = bookService;
         _bookshelfService = bookshelfService;
         _publisherService = publisherService;
         _authorService = authorService;
         _bookAuthorService = bookAuthorService;
      }

      public async Task<IActionResult> Index()
      {
         var books =  _bookService.GetAllBooks();
         return View(books);
      }

      [Authorize(Roles = "Admin")]
      [HttpGet]
      public async Task<IActionResult> ManageBooks()
      {
         var books =  _bookService.GetAllBooks();
         return View(books);
      }

      [Authorize(Roles = "Admin")]
      [HttpGet]
      public async Task<IActionResult> CreateBook()
      {
         ViewBag.Bookshelves =  _bookshelfService.GetAllBookshelves();
         ViewBag.Publishers =  _publisherService.GetAllPublishers();
         return View();
      }

      [Authorize(Roles = "Admin")]
      [HttpPost]
      public async Task<IActionResult> CreateBook(Book book, string authorNames)
      {
         if (ModelState.IsValid)
         {
             _bookService.AddBook(book);

            var authorNameList = authorNames.Split(',').Select(a => a.Trim()).ToList();
            foreach (var authorName in authorNameList)
            {
               var author =  _authorService.GetAuthorByName(authorName);
               if (author == null)
               {
                  author = new Author { Name = authorName };
                   _authorService.AddAuthor(author);
               }

               var bookAuthor = new BookAuthor
               {
                  BookISBN = book.ISBN,
                  IdAuthor = author.IdAuthor
               };
                _bookAuthorService.AddBookAuthor(bookAuthor);
            }

            return RedirectToAction("ManageBooks");
         }

         ViewBag.Bookshelves =  _bookshelfService.GetAllBookshelves();
         ViewBag.Publishers =  _publisherService.GetAllPublishers();
         return View(book);
      }

      [Authorize(Roles = "Admin")]
      [HttpGet]
      public async Task<IActionResult> EditBook(string id)
      {
         var book =  _bookService.GetBookByISBN(id);
         if (book == null)
         {
            return NotFound();
         }

         ViewBag.Bookshelves =  _bookshelfService.GetAllBookshelves();
         ViewBag.Publishers =  _publisherService.GetAllPublishers();

         var authors =  _bookAuthorService.GetAuthorsByBookISBN(id);
         ViewBag.AuthorNames = string.Join(", ", authors.Select(a => a.Name));

         return View(book);
      }

      [Authorize(Roles = "Admin")]
      [HttpPost]
      public async Task<IActionResult> EditBook(Book book, string authorNames)
      {
         if (ModelState.IsValid)
         {
             _bookService.UpdateBook(book);

            var existingAuthors =  _bookAuthorService.GetAuthorsByBookISBN(book.ISBN);
            foreach (var existingAuthor in existingAuthors)
            {
               var bookAuthor =  _bookAuthorService.GetBookAuthorByBookAndAuthor(book.ISBN, existingAuthor.IdAuthor);
                _bookAuthorService.DeleteBookAuthor(bookAuthor.IdBookAuthor);
            }

            var authorNameList = authorNames.Split(',').Select(a => a.Trim()).ToList();
            foreach (var authorName in authorNameList)
            {
               var author =  _authorService.GetAuthorByName(authorName);
               if (author == null)
               {
                  author = new Author { Name = authorName };
                   _authorService.AddAuthor(author);
               }

               var bookAuthor = new BookAuthor
               {
                  BookISBN = book.ISBN,
                  IdAuthor = author.IdAuthor
               };
                _bookAuthorService.AddBookAuthor(bookAuthor);
            }

            return RedirectToAction("ManageBooks");
         }

         ViewBag.Bookshelves =  _bookshelfService.GetAllBookshelves();
         ViewBag.Publishers =  _publisherService.GetAllPublishers();
         return View(book);
      }

      [Authorize(Roles = "Admin")]
      [HttpPost]
      public async Task<IActionResult> DeleteBook(string id)
      {
          _bookService.DeleteBook(id);
         return RedirectToAction("ManageBooks");
      }
   }
}
