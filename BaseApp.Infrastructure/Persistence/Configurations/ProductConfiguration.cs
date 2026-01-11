using BaseApp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseApp.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : BaseEntityConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.Name).IsRequired();
        }
    }
}
