using AutoMapper;
using BaseApp.Application.Common.Extentions;
using BaseApp.Application.Common.Interfaces;
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
            _mapper.Map(request.Product, product);
            _mapper.MapCollection(
                request.Items,
                product.Items,
                src => src.Id,
                dest => dest.Id
            );

            _unitOfWork.Repository<Product>().Update(product);
            await _unitOfWork.SaveChangesAsync();
            return product.Id;
        }
    }
}
