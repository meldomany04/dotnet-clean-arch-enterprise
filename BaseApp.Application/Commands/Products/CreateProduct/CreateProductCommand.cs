using MediatR;

namespace BaseApp.Application.Commands.Products.CreateProduct
{
    public record CreateProductCommand(string Name, decimal Price) : IRequest<int>;
}
