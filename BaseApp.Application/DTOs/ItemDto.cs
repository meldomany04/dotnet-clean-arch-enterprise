using BaseApp.Application.Common.Attributes;
using BaseApp.Application.DTOs.Common;

namespace BaseApp.Application.DTOs
{
    public class ItemDto : BaseEntityDto
    {
        public string Name { get; set; } = default!;
        public int ProductId { get; set; }
        public ProductDto Product { get; set; }
    }

    public class UpdateItemDto
    {
        public int Id { get; set; }
        [AutoBindRowVersion]
        public byte[] RowVersion { get; set; } = null;
        public string Name { get; set; } = default!;
        public int ProductId { get; set; }
    }

}
