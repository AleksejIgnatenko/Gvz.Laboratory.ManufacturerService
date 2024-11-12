using Gvz.Laboratory.ManufacturerService.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.ManufacturerService
{
    public class GvzLaboratoryManufacturerServiceDbContext : DbContext
    {
        public DbSet<ManufacturerEntity> Manufacturers { get; set; }
        public DbSet<PartyEntity> Parties { get; set; }

        public GvzLaboratoryManufacturerServiceDbContext(DbContextOptions<GvzLaboratoryManufacturerServiceDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //configuration db
        }
    }
}
