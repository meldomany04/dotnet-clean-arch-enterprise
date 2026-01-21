using BaseApp.Application.Commands.Items.CreateItem;
using BaseApp.Application.Common.Mappings.BaseMapping;
using BaseApp.Domain.Entities;

namespace BaseApp.Application.Common.Mappings.ItemMapping
{
    public class CreateItemMappingProfile
        : BaseCreateProfile<CreateItemCommand, Item>
    {
    }
}
