using Gvz.Laboratory.ManufacturerService.Abstractions;
using Gvz.Laboratory.ManufacturerService.Entities;
using Gvz.Laboratory.ManufacturerService.Exceptions;
using Gvz.Laboratory.ManufacturerService.Models;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.ManufacturerService.Repositories
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly GvzLaboratoryManufacturerServiceDbContext _context;

        public ManufacturerRepository(GvzLaboratoryManufacturerServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateManufacturerAsync(ManufacturerModel manufacturer)
        {
            var existingManufacturer = await _context.Manufacturers.FirstOrDefaultAsync(m => m.ManufacturerName.Equals(manufacturer.ManufacturerName));

            if (existingManufacturer != null) { throw new RepositoryException("Такой производитель уже есть"); }

            var manufacturerEntity = new ManufacturerEntity
            {
                Id = manufacturer.Id,
                ManufacturerName = manufacturer.ManufacturerName,
                DateCreate = DateTime.UtcNow,
            };

            await _context.Manufacturers.AddAsync(manufacturerEntity);
            await _context.SaveChangesAsync();

            return manufacturerEntity.Id;
        }

        public async Task<ManufacturerEntity?> GetManufacturerEntityByIdAsync(Guid manufacturerId)
        {
            return await _context.Manufacturers.FirstOrDefaultAsync(m => m.Id == manufacturerId);
        }

        public async Task<List<ManufacturerModel>> GetManufacturersAsync()
        {
            var manufacturerEntities = await _context.Manufacturers
                .AsNoTracking()
                .OrderByDescending(m => m.DateCreate)
                .ToListAsync();

            var manufacturers = manufacturerEntities.Select(m => ManufacturerModel.Create(
                m.Id, 
                m.ManufacturerName, 
                false).manufacturer).ToList();

            return manufacturers;
        }

        public async Task<(List<ManufacturerModel> manufacturers, int numberManufacturers)> GetManufacturersForPageAsync(int pageNumber)
        {
            var manufacturerEntities = await _context.Manufacturers
                .AsNoTracking()
                .OrderByDescending(m => m.DateCreate)
                .Skip(pageNumber * 20)
                .Take(20)
                .ToListAsync();

            var numberManufacturers = await _context.Manufacturers.CountAsync();

            var manufacturers = manufacturerEntities.Select(m => ManufacturerModel.Create(
                m.Id,
                m.ManufacturerName,
                false).manufacturer).ToList();

            return (manufacturers, numberManufacturers);
        }

        public async Task<Guid> UpdateManufacturerAsync(ManufacturerModel manufacturer)
        {
            await _context.Manufacturers
                .Where(m => m.Id == manufacturer.Id)
                .ExecuteUpdateAsync(m => m
                    .SetProperty(m => m.ManufacturerName, manufacturer.ManufacturerName)
                 );

            return manufacturer.Id;
        }

        public async Task DeleteManufacturersAsync(List<Guid> ids)
        {
            await _context.Manufacturers
                .Where(s => ids.Contains(s.Id))
                .ExecuteDeleteAsync();
        }
    }
}
