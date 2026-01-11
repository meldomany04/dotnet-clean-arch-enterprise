using BaseApp.Application.DTOs.Common;

namespace BaseApp.Application.DTOs
{
    public class ProductDto : BaseEntityDto
    {
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
    }

}
