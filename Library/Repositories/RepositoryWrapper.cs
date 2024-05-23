using Library.Models;
using Library.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
	public class RepositoryWrapper : IRepositoryWrapper
	{
		private LibraryContext _libraryContext;

		private IBookRepository? _bookRepository;
		private IUserRepository? _userRepository;
		private IReviewRepository? _reviewRepository;
		private IAuthorRepository? _authorRepository;
		private IBookshelfRepository? _bookshelfRepository;
		private IRequestRepository? _requestRepository;
		private IBookAuthorRepository? _bookAuthorRepository;
      private IPublisherRepository? _publisherRepository;
      private IReservedBookRepository? _reservedBookRepository;
      private IReturnRequestRepository? _returnRequestRepository;
      private IContactFormRepository? _contactFormRepository;

      public IContactFormRepository ContactFormRepository
      {
         get
         {
            if (_contactFormRepository == null)
            {
               _contactFormRepository = new ContactFormRepository(_libraryContext);
            }
            return _contactFormRepository;
         }
      }

      public IReturnRequestRepository ReturnRequestRepository
		{
	      get
	      {
		      if (_returnRequestRepository == null)
		      {
			      _returnRequestRepository = new ReturnRequestRepository(_libraryContext);
		      }
		      return _returnRequestRepository;
	      }
      }

		public IReservedBookRepository ReservedBookRepository
      {
         get
         {
            if (_reservedBookRepository == null)
            {
               _reservedBookRepository = new ReservedBookRepository(_libraryContext);
            }
            return _reservedBookRepository;
         }
      }

      public IPublisherRepository PublisherRepository
      {
			get
			{
				if (_publisherRepository == null)
				{
               _publisherRepository = new PublisherRepository(_libraryContext);
				}

				return _publisherRepository;
			}
		}
		public IBookAuthorRepository BookAuthorRepository
		{
			get
			{
				if (_bookAuthorRepository == null)
				{
					_bookAuthorRepository = new BookAuthorRepository(_libraryContext);
				}

				return _bookAuthorRepository;
			}
		}

		public IRequestRepository RequestRepository
		{
			get
			{
				if (_requestRepository == null)
				{
					_requestRepository = new RequestRepository(_libraryContext);
				}

				return _requestRepository;
			}
		}
		public IBookshelfRepository BookshelfRepository
		{
			get
			{
				if (_bookshelfRepository == null)
				{
					_bookshelfRepository = new BookshelfRepository(_libraryContext);
				}

				return _bookshelfRepository;
			}
		}

		public IAuthorRepository AuthorRepository
		{
			get
			{
				if (_authorRepository == null)
				{
					_authorRepository = new AuthorRepository(_libraryContext);
				}

				return _authorRepository;
			}
		}

		public IReviewRepository ReviewRepository
		{
			get
			{
				if (_reviewRepository == null)
				{
					_reviewRepository = new ReviewRepository(_libraryContext);
				}
				return _reviewRepository;
			}
		}
		public IBookRepository BookRepository
		{
			get
			{
				if (_bookRepository == null)
				{
					_bookRepository = new BookRepository(_libraryContext);
				}
				return _bookRepository;
			}
		}
		public IUserRepository UserRepository
		{
			get
			{
				if (_userRepository == null)
				{
					_userRepository = new UserRepository(_libraryContext);
				}
				return _userRepository;
			}
		}


		public RepositoryWrapper(LibraryContext libraryContext)
		{
			_libraryContext = libraryContext;
		}

		public void Save()
		{
			_libraryContext.SaveChanges();
		}
	}
}
