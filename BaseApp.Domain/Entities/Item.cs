using BaseApp.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseApp.Domain.Entities
{
    public class Item : BaseEntity, ISoftDelete
    {
        public string Name { get; set; }
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        public bool IsDeleted { get; set; }


        public static Item Create(string name, int productId)
        {
            return new Item
            {
                Name = name,
                ProductId = productId
            };
        }
    }
}
