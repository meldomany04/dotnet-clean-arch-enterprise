using BaseApp.Application.Common.Interfaces.IRepositories;
using BaseApp.Domain.Entities;
using BaseApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BaseApp.Infrastructure.Repositories
{
    internal class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<Product?> GetAllProductItems(int productId)
        {
            return await _context.Products.Include(e => e.Items).FirstOrDefaultAsync(e => e.Id == productId);
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<List<Product>> GetAllProductsByIds(List<int> productsIds)
        {
            return await _context.Products.Where(p => productsIds.Contains(p.Id)).ToListAsync();
        }

        public async Task<List<Product>> GetAllProductsItems()
        {
            return await _context.Products.Include(e => e.Items).ToListAsync();
        }

        public async Task<(IEnumerable<Product> Data, int TotalRecords)> GetPaged(
            int pageNumber, int pageSize, string? sortBy = null, bool descending = false,
            Expression<Func<Product, bool>>? filter = null)
        {
            var query = _context.Products.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrEmpty(sortBy))
            {
                var param = Expression.Parameter(typeof(Product), "x");
                var property = Expression.PropertyOrField(param, sortBy);
                var lambda = Expression.Lambda(property, param);

                var methodName = descending ? "OrderByDescending" : "OrderBy";
                var result = typeof(Queryable).GetMethods()
                    .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(Product), property.Type)
                    .Invoke(null, new object[] { query, lambda });

                query = (IQueryable<Product>)result!;
            }

            var totalRecords = await query.CountAsync();
            var data = await query.Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();

            return (data, totalRecords);
        }



        public Task<Product?> GetProductById(int productId)
        {
            return _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
        }

        public void UpdateProducts(List<Product> products)
        {
            _context.Products.UpdateRange(products);
        }
    }
}
