using MediatR;

namespace BaseApp.Application.Commands.Items.CreateItem
{
    public record CreateItemCommand(string Name, int ProductId) : IRequest<int>;
}
