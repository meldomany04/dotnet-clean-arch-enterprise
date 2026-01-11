using AutoMapper;
using BaseApp.Application.Common.Exceptions;
using BaseApp.Application.Common.Interfaces;
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
            throw new NotFoundException(_localizer["ProductNotFound"], 1);
            throw new NotFoundException("not found product", 1);
            //var product = new Product
            //{
            //    Name = request.Name,
            //    Price = request.Price
            //};

            var product = _mapper.Map<Product>(request);

            await _unitOfWork.Repository<Product>().AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            await _notificationHub.NotifyAllAsync(
                "ProductCreated",
                new { request.Name, request.Price }
            );

            return product.Id;
        }
    }
}