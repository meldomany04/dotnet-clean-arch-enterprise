using AutoMapper;
using BaseApp.Application.Common.Exceptions;
using BaseApp.Application.Common.Extentions;
using BaseApp.Application.Common.Interfaces;
using BaseApp.Application.Common.Realtime;
using BaseApp.Domain.Entities;
using MediatR;

namespace BaseApp.Application.Commands.Products.UpdateProductList
{
    public class UpdateProductListCommandHandler: IRequestHandler<UpdateProductListCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationHub _notificationHub;

        public UpdateProductListCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, INotificationHub notificationHub)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationHub = notificationHub;
        }

        public async Task<int> Handle(UpdateProductListCommand request, CancellationToken cancellationToken)
        {
            var productIds = request.Products.Select(p => p.Id).ToList();

            var productDetails = await _unitOfWork.Repository<Product>()
                .GetListAsync(e => productIds.Contains(e.Id));

            _mapper.MapCollection(
                request.Products,
                productDetails,
                src => src.Id,
                dest => dest.Id
            );

            _unitOfWork.Repository<Product>().UpdateRange(productDetails);
            await _unitOfWork.SaveChangesAsync();

            return 1;
        }
    }
}