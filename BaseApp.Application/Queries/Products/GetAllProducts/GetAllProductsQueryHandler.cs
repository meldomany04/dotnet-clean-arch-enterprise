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
        private readonly ICacheService _cacheService;

        public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<BaseResponse<List<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var availabletest = await _cacheService.GetAsync<string>("test");
            if(availabletest is null)
            {
                await _cacheService.SetAsync("test", "Hello");
            }

            //throw new NotFoundException("not found product", 1);
            var products = await _unitOfWork.Repository<Product>().GetAllAsync();
            return BaseResponse<List<ProductDto>>.Ok(_mapper.Map<List<ProductDto>>(products));
        }
    }

}
