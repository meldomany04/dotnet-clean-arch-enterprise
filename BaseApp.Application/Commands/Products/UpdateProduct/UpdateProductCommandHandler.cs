using AutoMapper;
using BaseApp.Application.Common.Exceptions;
using BaseApp.Application.Common.Interfaces;
using BaseApp.Application.Common.Realtime;
using BaseApp.Domain.Entities;
using MediatR;

namespace BaseApp.Application.Commands.Products.UpdateProduct
{
    public class UpdateProductCommandHandler: IRequestHandler<UpdateProductCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationHub _notificationHub;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, INotificationHub notificationHub)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationHub = notificationHub;
        }

        public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productDetails = await _unitOfWork.Repository<Product>().GetByIdAsync(request.Id);
            
            if (productDetails == null)
                throw new NotFoundException("There is no product with the same Id", request.Id);

            _mapper.Map(request, productDetails);


            _unitOfWork.Repository<Product>().Update(productDetails);
            await _unitOfWork.SaveChangesAsync();

            await _notificationHub.NotifyAllAsync(
                "ProductCreated",
                new { request.Name, request.Price }
            );

            return request.Id;
        }
    }
}