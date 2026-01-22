using AutoMapper;
using BaseApp.Application.Common.Exceptions;
using BaseApp.Application.Common.Extentions;
using BaseApp.Application.Common.Interfaces.IRepositories;
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

            var productDetails = await _unitOfWork.ProductRepository.GetAllProductsByIds(productIds);

            foreach (var product in productDetails)
            {
                var productToEdit = request.Products.FirstOrDefault(e => e.Id == product.Id);
                if (productToEdit == null)
                    throw new NotFoundException(nameof(Product), product.Id);

                product.Name = productToEdit.Name;
                product.Price = productToEdit.Price;
            }

            _unitOfWork.ProductRepository.UpdateProducts(productDetails);
            var result = await _unitOfWork.SaveChangesAsync();
            if(result > 0)
            {
                return productDetails.Count;
            }
            return 0;
        }
    }
}