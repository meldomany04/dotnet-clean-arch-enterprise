using BaseApp.Application.DTOs;
using MediatR;

namespace BaseApp.Application.Commands.Products.UpdateProductItems
{
    public class UpdateProductItemsCommand : IRequest<int>
    {
        public UpdateProductDto Product { get; set; }
    }
}
