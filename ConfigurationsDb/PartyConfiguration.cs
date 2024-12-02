using Gvz.Laboratory.ManufacturerService.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.ManufacturerService.ConfigurationsDb
{
    public class PartyConfiguration : IEntityTypeConfiguration<PartyEntity>
    {
        public void Configure(EntityTypeBuilder<PartyEntity> builder)
        {
            builder.Property(x => x.DateOfManufacture)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.DocumentOnQualityAndSafety)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(x => x.TestReport)
                .IsRequired()
                .HasMaxLength(128);
        }
    }
}
