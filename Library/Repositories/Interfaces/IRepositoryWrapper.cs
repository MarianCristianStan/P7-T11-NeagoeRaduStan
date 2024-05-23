namespace Library.Repositories.Interfaces
{
	public interface IRepositoryWrapper
	{

		IBookRepository BookRepository { get; }
		IUserRepository UserRepository { get; }
		IReviewRepository ReviewRepository { get; }
		IAuthorRepository AuthorRepository { get; }
		IBookshelfRepository BookshelfRepository { get; }
		IRequestRepository RequestRepository { get; }
		IBookAuthorRepository BookAuthorRepository { get; }
		IPublisherRepository PublisherRepository { get; }
      IReservedBookRepository ReservedBookRepository { get; }
		IReturnRequestRepository ReturnRequestRepository { get; }
		IContactFormRepository ContactFormRepository { get; }
      void Save();
	}
}
