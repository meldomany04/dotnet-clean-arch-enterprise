using AutoMapper;
using BaseApp.Application.Common.Exceptions;
using BaseApp.Application.Common.Extentions;
using BaseApp.Application.Common.Interfaces.IRepositories;
using BaseApp.Application.Common.Responses;
using BaseApp.Application.DTOs;
using BaseApp.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace BaseApp.Application.Queries.Products.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResponse<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Product, bool>> filter = p => true;

            filter = filter
                .AndAlsoIf(!string.IsNullOrEmpty(request.Name), p => p.Name != null && p.Name.Contains(request.Name))
                .AndAlsoIf(request.MinPrice.HasValue, p => p.Price >= request.MinPrice)
                .AndAlsoIf(request.MaxPrice.HasValue, p => p.Price <= request.MaxPrice);

            var (data, totalRecords) = await _unitOfWork.ProductRepository.GetPaged(request.PageNumber, request.PageSize, request.SortBy, request.SortDescending, filter);

            var dtoData = _mapper.Map<List<ProductDto>>(data);

            return PagedResponse<ProductDto>.Create(dtoData, request.PageNumber, request.PageSize, totalRecords);
        }
    }

}
