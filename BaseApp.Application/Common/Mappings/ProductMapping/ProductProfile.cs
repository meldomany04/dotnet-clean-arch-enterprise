using AutoMapper;
using BaseApp.Application.DTOs;
using BaseApp.Domain.Entities;

namespace BaseApp.Application.Common.Mappings.ProductMapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}
