using BaseApp.Domain.Common;

namespace BaseApp.Domain.Entities
{
    public class Product : AuditableEntity, ISoftDelete
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
    }
}
