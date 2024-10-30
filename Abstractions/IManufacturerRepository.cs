using Gvz.Laboratory.ManufacturerService.Models;

namespace Gvz.Laboratory.ManufacturerService.Abstractions
{
    public interface IManufacturerRepository
    {
        Task<Guid> CreateManufacturerAsync(ManufacturerModel manufacturer);
        Task DeleteManufacturersAsync(List<Guid> ids);
        Task<(List<ManufacturerModel> manufacturers, int numberManufacturers)> GetManufacturersForPageAsync(int pageNumber);
        Task<Guid> UpdateManufacturerAsync(ManufacturerModel manufacturer);
    }
}