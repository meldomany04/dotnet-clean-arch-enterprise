using BaseApp.Application.Commands.Products.UpdateProduct;
using BaseApp.Application.Common.Mappings.BaseMapping;
using BaseApp.Domain.Entities;

namespace BaseApp.Application.Common.Mappings.ProductMapping
{
    public class UpdateProductMappingProfile
        : BaseModifyProfile<UpdateProductCommand, Product>
    {
    }

}
