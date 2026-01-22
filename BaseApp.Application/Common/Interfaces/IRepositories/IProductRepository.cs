using BaseApp.Domain.Entities;
using System.Linq.Expressions;

namespace BaseApp.Application.Common.Interfaces.IRepositories
{
    public interface IProductRepository
    {
        Task AddProduct(Product product);
        void DeleteProduct(Product product);
        Task<Product?> GetProductById(int productId);
        void UpdateProduct(Product product);
        Task<List<Product>> GetAllProductsByIds(List<int> productsIds);
        void UpdateProducts(List<Product> products);
        Task<List<Product>> GetAllProductsItems();
        Task<List<Product>> GetAllProducts();
        Task<Product?> GetAllProductItems(int productId);
        Task<(IEnumerable<Product> Data, int TotalRecords)> GetPaged(
            int pageNumber, int pageSize, string? sortBy = null, bool descending = false,
            Expression<Func<Product, bool>>? filter = null);
    }
}
