using System.Linq.Expressions;
using Library.Models;
using Library.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
	public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
	{
		protected LibraryContext LibraryContext { get; set; }

		public RepositoryBase(LibraryContext libraryContext)
		{
			this.LibraryContext = libraryContext;
		}

		public IQueryable<T> FindAll()
		{
			return this.LibraryContext.Set<T>().AsNoTracking();
		}

		public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
		{
			return this.LibraryContext.Set<T>().Where(expression).AsNoTracking();
		}

		public void Create(T entity)
		{
			this.LibraryContext.Set<T>().Add(entity);
		}

		public void Update(T entity)
		{
			this.LibraryContext.Set<T>().Update(entity);
		}

		public void Delete(T entity)
		{
			this.LibraryContext.Set<T>().Remove(entity);
		}
	}
}
