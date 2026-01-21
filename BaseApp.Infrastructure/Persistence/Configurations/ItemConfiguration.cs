using BaseApp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseApp.Infrastructure.Persistence.Configurations
{
    public class ItemConfiguration : BaseEntityConfiguration<Item>
    {
        public override void Configure(EntityTypeBuilder<Item> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.Name).IsRequired();
        }
    }
}
