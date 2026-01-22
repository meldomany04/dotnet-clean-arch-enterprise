namespace BaseApp.Application.Common.Interfaces.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository ProductRepository { get; }
        IItemRepository ItemRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
