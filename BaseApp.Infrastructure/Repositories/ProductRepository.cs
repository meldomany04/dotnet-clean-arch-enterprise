using BaseApp.Application.Common.Interfaces;
using BaseApp.Domain.Entities;
using BaseApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<Product>> GetAllProductsItems()
        {
            return await _context.Products.Include(e => e.Items).ToListAsync();
        }

        public async Task<Product> GetAllProductItems(int productId)
        {
            return await _context.Products.Include(e => e.Items).FirstAsync(e => e.Id == productId);
        }
    }
}
