using BaseApp.Domain.Common;

namespace BaseApp.Domain.Entities
{
    public class Product : BaseEntity, ISoftDelete
    {
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
    }
}
