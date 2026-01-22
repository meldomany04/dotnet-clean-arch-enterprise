using BaseApp.Application.Common.Exceptions;
using BaseApp.Application.Common.Interfaces.IRepositories;
using BaseApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();


        private IProductRepository? _products;
        public IProductRepository ProductRepository => _products ??= new ProductRepository(_context);


        private IItemRepository? _items;
        public IItemRepository ItemRepository => _items ??= new ItemRepository(_context);

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
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

        public void Dispose() => _context.Dispose();
    }

}
