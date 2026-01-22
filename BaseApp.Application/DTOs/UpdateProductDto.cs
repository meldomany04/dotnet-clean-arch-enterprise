using BaseApp.Application.Common.Attributes;

namespace BaseApp.Application.DTOs
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        [AutoBindRowVersion]
        public byte[] RowVersion { get; set; }
        public List<UpdateItemDto> Items { get; set; }
    }
}
