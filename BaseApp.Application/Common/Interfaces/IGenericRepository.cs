using System.Linq.Expressions;

namespace BaseApp.Application.Common.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        IQueryable<T> GetAll();
        Task<IReadOnlyList<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<(IEnumerable<T> Data, int TotalRecords)> GetPagedAsync(
                int pageNumber, int pageSize, string? sortBy = null, bool descending = false,
                Expression<Func<T, bool>>? filter = null);
    }
}
