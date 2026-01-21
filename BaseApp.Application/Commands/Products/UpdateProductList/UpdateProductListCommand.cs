using BaseApp.Application.DTOs;
using MediatR;

namespace BaseApp.Application.Commands.Products.UpdateProductList
{
    public class UpdateProductListCommand : IRequest<int>
    {
        public List<UpdateProductDto> Products { get; set; }
    }
}