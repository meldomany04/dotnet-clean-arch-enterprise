using BaseApp.Application.Common.Exceptions;
using BaseApp.Application.Common.Interfaces;
using BaseApp.Domain.Common;
using BaseApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new GenericRepository<T>(_context);
            }

            return (IGenericRepository<T>)_repositories[type];
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException("The record was modified by another user. Please reload and try again.");
            }
        }

        public void SetOriginalRowVersion<T>(T entity, byte[] originalRowVersion) where T : class
        {
            if (entity is BaseEntity baseEntity)
            {
                _context.Entry(entity).Property(nameof(BaseEntity.RowVersion)).OriginalValue = originalRowVersion;
            }
        }

        public void Dispose() => _context.Dispose();
    }

}
