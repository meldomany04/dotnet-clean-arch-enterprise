using BaseApp.Application.Common.Exceptions;
using BaseApp.Application.Common.Interfaces.IRepositories;
using BaseApp.Domain.Entities;
using MediatR;

namespace BaseApp.Application.Commands.Products.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var productDetails = await _unitOfWork.ProductRepository.GetProductById(request.Id);
            if (productDetails is null)
                throw new NotFoundException("There is no prodct with this Id", request.Id);

            _unitOfWork.ProductRepository.DeleteProduct(productDetails);
            await _unitOfWork.SaveChangesAsync();
            return request.Id;
        }
    }
}