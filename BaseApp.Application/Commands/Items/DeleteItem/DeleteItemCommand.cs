using BaseApp.Application.Common.Attributes;
using MediatR;

namespace BaseApp.Application.Commands.Products.DeleteProduct
{
    public class DeleteItemCommand : IRequest<int>
    {
        public int Id { get; set; }

        [AutoBindRowVersion]
        public byte[] RowVersion { get; set; }
    }
}
