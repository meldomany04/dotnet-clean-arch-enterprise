using BaseApp.Domain.Common;

namespace BaseApp.Domain.Entities
{
    public class Product : BaseEntity, ISoftDelete
    {
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public List<Item> Items { get; set; }
        public bool IsDeleted { get; set; }

        public static Product Create(string name, decimal price)
        {
            return new Product
            {
                Name = name,
                Price = price,
                Items = new List<Item>(),
                IsDeleted = false
            };
        }
    }
}
