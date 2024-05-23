using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Microsoft.AspNetCore.Identity;
using Library.Services.Interfaces;
using System.Threading.Tasks;
using Library.Services;
using Microsoft.AspNetCore.Authorization;

namespace Library.Controllers
{
   [Authorize(Roles = "Admin,Basic")]
   public class RequestController : Controller
   {
      private readonly UserManager<User> _userManager;
      private readonly IRequestService _requestService;
      private readonly IUserService _userService;
      private readonly IBookService _bookService;
      private readonly IBookAuthorService _bookAuthorService;
      private readonly IAuthorService _authorService;
      private readonly IReservedBookService _reservedBookService;
      private readonly IReturnRequestService _returnRequestService;
      public RequestController(UserManager<User> userManager, IRequestService requestService, IUserService userService,
         IBookService bookService, IBookAuthorService bookAuthorService, IAuthorService authorService, IReservedBookService reservedBookService, IReturnRequestService returnRequestService)
      {
         _userManager = userManager;
         _requestService = requestService;
         _userService = userService;
         _bookService = bookService;
         _bookAuthorService = bookAuthorService;
         _authorService = authorService;
         _reservedBookService = reservedBookService;
         _returnRequestService = returnRequestService;
      }

      [HttpGet]
		
		public async Task<IActionResult> Create(string isbn)
      {
         var book = _bookService.GetBookByISBN(isbn);
         if (book == null)
         {
            return NotFound();
         }

         ViewBag.BookISBN = isbn;
         ViewBag.BookTitle = book.Title;
         ViewBag.BookCategory = book.Category;
         ViewBag.BookReleaseYear = book.ReleaseYear;
         var bookAuthors = _bookAuthorService.GetBookAuthorsByISBN(isbn);
         var authors = new List<string>();

         foreach (var bookAuthor in bookAuthors)
         {
            if (bookAuthor.IdAuthor != null)
            {
               var author = _authorService.GetAuthorById(bookAuthor.IdAuthor);
               if (author != null)
               {
                  authors.Add(author.Name);
               }
            }

         }

         ViewBag.BookAuthors = authors;

         return View();


      }

      [HttpPost]
      public async Task<IActionResult> Create(Request request)
      {
         var user = _userService.GetCurrentUser();

         if (user == null)
         {
            return Challenge();
         }

         var userRequest = new Request()
         {
            IdUser = user.Id,
            BookISBN = request.BookISBN,
            RequestDate = request.RequestDate,
            Description = request.Description,
            IsApproved = false
         };


         _requestService.AddRequest(userRequest);

         return RedirectToAction("Index", "Bookshelf");
      }


      [HttpGet]
      [Authorize(Roles = "Admin")]
		public async Task<IActionResult> AdminRequests()
      {
         var requests =  _requestService.GetAllRequests();
         return View(requests);
      }

     
      [HttpPost]
      [Authorize(Roles = "Admin")]
		public async Task<IActionResult> AcceptRequest(int requestId)
      {
         var request =  _requestService.GetRequestById(requestId);
         if (request == null)
         {
            return NotFound();
         }

         var reservedBook = new ReservedBook
         {
            IdUser = request.IdUser,
            BookISBN = request.BookISBN
         };

         _reservedBookService.AddReservedBook(reservedBook);

         request.IsApproved = true;
         _requestService.UpdateRequest(request);

         return RedirectToAction("AdminRequests");
      }

      [HttpPost]
      [Authorize(Roles = "Admin")]
		public async Task<IActionResult> DisableRequest(int requestId)
      {
	      var request =  _requestService.GetRequestById(requestId);
	      if (request == null)
	      {
		      return NotFound();
	      }

         var returnRequest = await _returnRequestService.GetReturnRequestByUserAndBookISBNAsync(request.IdUser, request.BookISBN);
         if (returnRequest != null)
         {
            TempData["Message"] = "The user has requested to return this book.";
            return RedirectToAction("AdminRequests"); 
         }

         var reservedBook = await _reservedBookService.GetReservedBookByUserAndISBNAsync(request.IdUser, request.BookISBN);
	      if (reservedBook != null)
	      {
		       _reservedBookService.DeleteReservedBook(reservedBook.IdReservedBook);
	      }

	      request.IsApproved = false;
	       _requestService.UpdateRequest(request);

	      return RedirectToAction("AdminRequests");
      }


      [HttpGet]
      [Authorize(Roles = "Admin")]
      public async Task<IActionResult> AdminReturnRequests()
      {
         var requests = await _returnRequestService.GetAllReturnRequestsAsync();
         return View(requests);
      }



      [HttpPost]
      [Authorize(Roles = "Admin")]
      public async Task<IActionResult> AcceptReturnRequest(int requestId)
      {
         var returnRequest = await _returnRequestService.GetReturnRequestByIdAsync(requestId);
         if (returnRequest == null)
         {
            return NotFound();
         }

         var reservedBook = await _reservedBookService.GetReservedBookByUserAndISBNAsync(returnRequest.IdUser, returnRequest.BookISBN);
         if (reservedBook != null)
         {
            _reservedBookService.DeleteReservedBook(reservedBook.IdReservedBook);
         }

         await _returnRequestService.DeleteReturnRequestAsync(requestId);

         var originalRequest = _requestService.GetRequestByUserAndBookISBN(returnRequest.IdUser, returnRequest.BookISBN);
         if (originalRequest != null)
         {
            _requestService.DeleteRequest(originalRequest.IdRequest);
         }

         await _returnRequestService.DeleteReturnRequestAsync(requestId);
         return RedirectToAction("AdminReturnRequests");
      }

      [HttpPost]
      [Authorize(Roles = "Admin")]
      public async Task<IActionResult> RejectReturnRequest(int requestId)
      {
         await _returnRequestService.DeleteReturnRequestAsync(requestId);
         return RedirectToAction("AdminReturnRequests");
      }





   }
}