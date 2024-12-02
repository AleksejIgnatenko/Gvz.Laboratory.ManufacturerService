using Gvz.Laboratory.ManufacturerService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gvz.Laboratory.ManufacturerService.ConfigurationsDb
{
    public class ManufacturerConfiguration : IEntityTypeConfiguration<ManufacturerEntity>
    {
        public void Configure(EntityTypeBuilder<ManufacturerEntity> builder)
        {
            builder.Property(x => x.ManufacturerName)
                .IsRequired()
                .HasMaxLength(128);
        }
    }
}
