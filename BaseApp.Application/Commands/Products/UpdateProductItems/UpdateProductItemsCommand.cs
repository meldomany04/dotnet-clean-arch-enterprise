using BaseApp.Application.DTOs;
using MediatR;

namespace BaseApp.Application.Commands.Products.UpdateProductItems
{
    public class UpdateProductItemsCommand : IRequest<int>
    {
        public ProductDto Product { get; set; }
        public List<UpdateItemDto> Items{ get; set; }
    }
}
