using AutoMapper;
using BaseApp.Application.DTOs;
using BaseApp.Domain.Entities;

namespace BaseApp.Application.Common.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }

}
