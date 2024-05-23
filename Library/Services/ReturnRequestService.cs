using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
	public class ReturnRequestService : IReturnRequestService
	{
		private readonly IRepositoryWrapper _repositoryWrapper;


      public async Task<ReturnRequest> GetReturnRequestByIdAsync(int requestId) 
      {
         return await _repositoryWrapper.ReturnRequestRepository.FindByCondition(r => r.IdRequest == requestId).FirstOrDefaultAsync();
      }

      public ReturnRequestService(IRepositoryWrapper repositoryWrapper)
		{
			_repositoryWrapper = repositoryWrapper;
		}
      public async Task<ReturnRequest> GetReturnRequestByUserAndBookISBNAsync(string userId, string bookISBN)
      {
         return await _repositoryWrapper.ReturnRequestRepository.FindByCondition(r => r.IdUser == userId && r.BookISBN == bookISBN).FirstOrDefaultAsync();
      }
      public async Task<IEnumerable<ReturnRequest>> GetAllReturnRequestsAsync()
		{
			return await _repositoryWrapper.ReturnRequestRepository.FindAll().ToListAsync();
		}


      public async Task AddReturnRequestAsync(ReturnRequest returnRequest)
      {
         _repositoryWrapper.ReturnRequestRepository.Create(returnRequest);
         _repositoryWrapper.Save();
      }

      public async Task DeleteReturnRequestAsync(int requestId)
      {
         var request = await _repositoryWrapper.ReturnRequestRepository.FindByCondition(r => r.IdRequest == requestId).FirstOrDefaultAsync();
         if (request != null)
         {
            _repositoryWrapper.ReturnRequestRepository.Delete(request);
            _repositoryWrapper.Save();
         }
      }
   }
}
