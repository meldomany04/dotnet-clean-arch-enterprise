using AutoMapper;
using BaseApp.Application.Common.Exceptions;
using BaseApp.Application.Common.Extentions;
using BaseApp.Application.Common.Interfaces.IRepositories;
using BaseApp.Application.DTOs;
using BaseApp.Domain.Entities;
using MediatR;

namespace BaseApp.Application.Commands.Products.UpdateProductItems
{
    public class UpdateProductItemsCommandHandler : IRequestHandler<UpdateProductItemsCommand, int>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateProductItemsCommandHandler(IProductRepository productRepository, 
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(UpdateProductItemsCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAllProductItems(request.Product.Id);

            if (product == null)
                throw new NotFoundException("Product", request.Product.Id);
            
            product.Name = request.Product.Name;
            product.Price = request.Product.Price;

            foreach (var item in request.Product.Items)
            {
                var productItem = product.Items.FirstOrDefault(i => i.Id == item.Id);
                if (productItem != null)
                {
                    productItem.Name = item.Name;
                    productItem.ProductId = product.Id;
                    productItem.Product = product;
                }
            }

            _unitOfWork.ProductRepository.UpdateProduct(product);
            var result = await _unitOfWork.SaveChangesAsync();
            
            if(result > 0)
            {
                return product.Id;
            }
            return 0;
        }
    }
}
