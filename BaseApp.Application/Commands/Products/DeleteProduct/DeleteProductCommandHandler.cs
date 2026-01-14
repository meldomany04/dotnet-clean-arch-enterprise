using BaseApp.Application.Common.Exceptions;
using BaseApp.Application.Common.Interfaces;
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
            var productDetails = await _unitOfWork.Repository<Product>().GetByIdAsync(request.Id);
            if (productDetails == null)
                throw new NotFoundException("There is no prodct with this Id", request.Id);

            _unitOfWork.Repository<Product>().Remove(productDetails);
            await _unitOfWork.SaveChangesAsync();
            return request.Id;
        }
    }
}