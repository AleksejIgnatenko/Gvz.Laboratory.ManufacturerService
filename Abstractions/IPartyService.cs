using Gvz.Laboratory.ManufacturerService.Models;

namespace Gvz.Laboratory.ManufacturerService.Abstractions
{
    public interface IPartyService
    {
        Task<(List<PartyModel> parties, int numberParties)> GetManufacturerPartiesForPageAsync(Guid manufacturerId, int pageNumber);
    }
}