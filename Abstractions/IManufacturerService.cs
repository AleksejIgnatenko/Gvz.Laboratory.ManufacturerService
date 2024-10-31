using Gvz.Laboratory.ManufacturerService.Models;

namespace Gvz.Laboratory.ManufacturerService.Abstractions
{
    public interface IManufacturerService
    {
        Task<Guid> CreateManufacturerAsync(Guid id, string manufacturerName);
        Task DeleteManufacturersAsync(List<Guid> ids);
        Task<List<ManufacturerModel>> GetManufacturersAsync();
        Task<(List<ManufacturerModel> manufacturers, int numberManufacturers)> GetManufacturersForPageAsync(int pageNumber);
        Task<Guid> UpdateManufacturerAsync(Guid id, string manufacturerName);
    }
}