using BaseApp.Domain.Entities;

namespace BaseApp.Application.Common.Interfaces.IRepositories
{
    public interface IItemRepository
    {
        Task AddItem(Item item);
        void RemoveItem(Item item);
        Task<Item?> GetItemById(int id);
        Task<List<Item>> GetAllItems();
    }
}
