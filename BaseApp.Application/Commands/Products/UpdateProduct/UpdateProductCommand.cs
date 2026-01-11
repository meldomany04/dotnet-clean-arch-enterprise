using MediatR;

namespace BaseApp.Application.Commands.Products.UpdateProduct
{
    public record UpdateProductCommand(int Id, string Name, decimal Price, byte[] RowVersion) : IRequest<int>;
}