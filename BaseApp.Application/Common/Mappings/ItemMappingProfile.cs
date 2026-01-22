using AutoMapper;
using BaseApp.Application.Commands.Items.CreateItem;
using BaseApp.Application.DTOs;
using BaseApp.Domain.Entities;

namespace BaseApp.Application.Common.Mappings
{
    public class ItemMappingProfile : Profile
    {
        public ItemMappingProfile()
        {
            CreateMap<Item, ItemDto>();
            CreateMap<ItemDto, Item>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());
        }
    }

}
