using BaseApp.Application.Common.Interfaces.IRepositories;
using BaseApp.Domain.Entities;
using BaseApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext context;

        public ItemRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddItem(Item item)
        {
            await context.Items.AddAsync(item);
        }

        public void RemoveItem(Item item)
        {
            context.Items.Remove(item);
        }

        public async Task<List<Item>> GetAllItems()
        {
            return await context.Items.ToListAsync();
        }

        public async Task<Item?> GetItemById(int id)
        {
            return await context.Items.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
