using AutoMapper;
using BaseApp.Application.Common.Interfaces;
using BaseApp.Application.Common.Responses;
using BaseApp.Application.DTOs;
using BaseApp.Domain.Entities;
using MediatR;

namespace BaseApp.Application.Queries.Products.GetAllProducts
{
    public class GetAllProductsQueryHandler
        : IRequestHandler<GetAllProductsQuery, BaseResponse<List<ProductDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            //throw new NotFoundException("not found product", 1);
            var products = await _unitOfWork.Repository<Product>().GetAllAsync();
            return BaseResponse<List<ProductDto>>.Ok(_mapper.Map<List<ProductDto>>(products));
        }
    }

}
