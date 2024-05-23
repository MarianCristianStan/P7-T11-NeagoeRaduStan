using Library.Models;

namespace Library.Services.Interfaces
{
	public interface IReturnRequestService
	{
		Task<IEnumerable<ReturnRequest>> GetAllReturnRequestsAsync();
      Task AddReturnRequestAsync(ReturnRequest returnRequest);
      Task DeleteReturnRequestAsync(int requestId);
      Task<ReturnRequest> GetReturnRequestByIdAsync(int requestId);
      Task<ReturnRequest> GetReturnRequestByUserAndBookISBNAsync(string userId, string bookISBN);
   }
}
