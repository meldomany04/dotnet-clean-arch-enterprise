using AutoMapper;
using BaseApp.Application.Common.Interfaces;
using BaseApp.Application.Common.Interfaces.IRepositories;
using BaseApp.Application.Common.Responses;
using BaseApp.Application.DTOs;
using BaseApp.Domain.Entities;
using MediatR;

namespace BaseApp.Application.Queries.Items.GetAllItems
{
    public class GetAllItemsQueryHandler
        : IRequestHandler<GetAllItemsQuery, BaseResponse<List<ItemDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetAllItemsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<BaseResponse<List<ItemDto>>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
        {
            var Items = await _unitOfWork.ItemRepository.GetAllItems();
            return BaseResponse<List<ItemDto>>.Ok(_mapper.Map<List<ItemDto>>(Items));
        }
    }

}
