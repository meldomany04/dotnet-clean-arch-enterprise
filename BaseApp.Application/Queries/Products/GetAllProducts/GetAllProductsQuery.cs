using BaseApp.Application.Common.Responses;
using BaseApp.Application.DTOs;
using MediatR;

namespace BaseApp.Application.Queries.Products.GetAllProducts
{
    public record GetAllProductsQuery : IRequest<BaseResponse<List<ProductDto>>>;
}
