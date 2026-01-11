using MediatR;

namespace BaseApp.Application.Commands.Products.DeleteProduct
{
    public record DeleteProductCommand(int Id, byte[] RowVersion) : IRequest<int>;
}
