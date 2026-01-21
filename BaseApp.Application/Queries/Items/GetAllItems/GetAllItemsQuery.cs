using BaseApp.Application.Common.Responses;
using BaseApp.Application.DTOs;
using MediatR;

namespace BaseApp.Application.Queries.Items.GetAllItems
{
    public record GetAllItemsQuery : IRequest<BaseResponse<List<ItemDto>>>;
}
