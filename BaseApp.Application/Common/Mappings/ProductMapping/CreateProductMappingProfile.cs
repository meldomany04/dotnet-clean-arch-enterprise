using BaseApp.Application.Commands.Products.CreateProduct;
using BaseApp.Application.Common.Mappings.BaseMapping;
using BaseApp.Domain.Entities;

namespace BaseApp.Application.Common.Mappings.ProductMapping
{
    public class CreateProductMappingProfile
        : BaseCreateProfile<CreateProductCommand, Product>
    {
    }

}
