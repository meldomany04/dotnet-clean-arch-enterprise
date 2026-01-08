using BaseApp.Application.Common.Exceptions;
using BaseApp.Application.Common.Extentions;
using BaseApp.Application.Common.Interfaces;
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

        public GetProductsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResponse<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            //throw new UnauthorizedException();
            Expression<Func<Product, bool>> filter = p => true;

            if (!string.IsNullOrEmpty(request.Name))
                filter = filter.AndAlso(p => p.Name != null && p.Name.Contains(request.Name));

            if (request.MinPrice.HasValue)
                filter = filter.AndAlso(p => p.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                filter = filter.AndAlso(p => p.Price <= request.MaxPrice.Value);


            var (data, totalRecords) = await _unitOfWork.Repository<Product>()
                .GetPagedAsync(request.PageNumber, request.PageSize, request.SortBy, request.SortDescending, filter);

            var dtoData = data.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            });

            return PagedResponse<ProductDto>.Create(dtoData, request.PageNumber, request.PageSize, totalRecords);

        }
    }

}
