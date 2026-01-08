using BaseApp.Application.Common.Requests;
using BaseApp.Application.Common.Responses;
using BaseApp.Application.DTOs;
using MediatR;

namespace BaseApp.Application.Queries.Products.GetProducts
{
    public class GetProductsQuery : PaginationQuery, IRequest<PagedResponse<ProductDto>>
    {
        public string? Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
