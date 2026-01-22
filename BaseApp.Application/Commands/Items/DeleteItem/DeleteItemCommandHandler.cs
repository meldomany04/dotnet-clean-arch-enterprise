using BaseApp.Application.Commands.Products.DeleteProduct;
using BaseApp.Application.Common.Exceptions;
using BaseApp.Application.Common.Interfaces.IRepositories;
using MediatR;

namespace BaseApp.Application.Commands.Items.DeleteItem
{
    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteItemCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var ItemDetails = await _unitOfWork.ItemRepository.GetItemById(request.Id);
            if (ItemDetails == null)
                throw new NotFoundException("There is no item with this Id", request.Id);

            _unitOfWork.ItemRepository.RemoveItem(ItemDetails);
            await _unitOfWork.SaveChangesAsync();
            return request.Id;
        }
    }
}