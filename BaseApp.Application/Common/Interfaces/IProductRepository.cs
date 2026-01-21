using BaseApp.Domain.Entities;

namespace BaseApp.Application.Common.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsItems();
        Task<Product> GetAllProductItems(int productId);
    }
}
