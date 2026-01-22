using AutoMapper;
using BaseApp.Application.Common.Exceptions;
using BaseApp.Application.Common.Interfaces.IRepositories;
using BaseApp.Application.Common.Realtime;
using BaseApp.Application.Resources;
using BaseApp.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BaseApp.Application.Commands.Products.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly INotificationHub _notificationHub;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork,
            IMapper mapper,
            IStringLocalizer<SharedResource> localizer,
            INotificationHub notificationHub)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _notificationHub = notificationHub;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = Product.Create(request.Name, request.Price);

            await _unitOfWork.ProductRepository.AddProduct(product);
            await _unitOfWork.SaveChangesAsync();

            await _notificationHub.NotifyAllAsync(
                "ProductCreated",
                new { request.Name, request.Price }
            );

            return product.Id;
        }
    }
}