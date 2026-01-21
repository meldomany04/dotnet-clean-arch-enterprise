using BaseApp.Application.Common.Interfaces;
using BaseApp.Domain.Common;
using BaseApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BaseApp.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);

        public async Task<IReadOnlyList<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();
        
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>>? filter = null,
                                        CancellationToken cancellationToken = default)
        {
            var query = _context.Set<T>().AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

        public void Update(T entity) => _context.Set<T>().Update(entity);

        public void UpdateRange(List<T> entities) => _context.Set<T>().UpdateRange(entities);

        public IQueryable<T> GetAll() => _context.Set<T>().AsNoTracking();

        public async Task<(IEnumerable<T> Data, int TotalRecords)> GetPagedAsync(
            int pageNumber, int pageSize, string? sortBy = null, bool descending = false,
            Expression<Func<T, bool>>? filter = null)
        {
            var query = _context.Set<T>().AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrEmpty(sortBy))
            {
                var param = Expression.Parameter(typeof(T), "x");
                var property = Expression.PropertyOrField(param, sortBy);
                var lambda = Expression.Lambda(property, param);

                var methodName = descending ? "OrderByDescending" : "OrderBy";
                var result = typeof(Queryable).GetMethods()
                    .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), property.Type)
                    .Invoke(null, new object[] { query, lambda });

                query = (IQueryable<T>)result!;
            }

            var totalRecords = await query.CountAsync();
            var data = await query.Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();

            return (data, totalRecords);
        }

        public void Remove(T entity)
        {
            if (entity is ISoftDelete softDeleteEntity)
            {
                softDeleteEntity.IsDeleted = true;
                var entry = _context.Entry(entity);

                if (entry.State == EntityState.Detached)
                {
                    _context.Set<T>().Attach(entity);
                    entry = _context.Entry(entity);
                }

                entry.State = EntityState.Modified;
            }
            else
            {
                _context.Set<T>().Remove(entity);
            }
        }
    }

}
