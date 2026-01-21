using AutoMapper;
using BaseApp.Application.Common.Mappings.BaseMapping;
using BaseApp.Application.DTOs;
using BaseApp.Domain.Entities;

namespace BaseApp.Application.Common.Mappings.ItemMapping
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, ItemDto>();
        }
    }


    public class UpdateProductListMappingProfile
: BaseModifyProfile<UpdateItemDto, Item>
    {
    }
}
