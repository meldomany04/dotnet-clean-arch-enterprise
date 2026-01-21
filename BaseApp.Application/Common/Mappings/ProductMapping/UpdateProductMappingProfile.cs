using BaseApp.Application.Commands.Products.UpdateProduct;
using BaseApp.Application.Commands.Products.UpdateProductList;
using BaseApp.Application.Common.Mappings.BaseMapping;
using BaseApp.Application.DTOs;
using BaseApp.Domain.Entities;

namespace BaseApp.Application.Common.Mappings.ProductMapping
{
    public class UpdateProductMappingProfile
        : BaseModifyProfile<UpdateProductCommand, Product>
    {
    }

    public class UpdateProductListMappingProfile
    : BaseModifyProfile<UpdateProductDto, Product>
    {
    }

}
