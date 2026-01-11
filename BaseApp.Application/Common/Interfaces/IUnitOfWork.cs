namespace BaseApp.Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync();
        void SetOriginalRowVersion<T>(T entity, byte[] originalRowVersion) where T : class;
    }
}
