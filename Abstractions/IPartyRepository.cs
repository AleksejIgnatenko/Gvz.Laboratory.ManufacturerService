using Gvz.Laboratory.ManufacturerService.Dto;
using Gvz.Laboratory.ManufacturerService.Models;

namespace Gvz.Laboratory.ManufacturerService.Abstractions
{
    public interface IPartyRepository
    {
        Task<Guid> CreatePartyAsync(PartyDto party);
        Task DeletePartiesAsync(List<Guid> ids);
        Task<(List<PartyModel> parties, int numberParties)> GetManufacturerPartiesForPageAsync(Guid manufacturerId, int pageNumber);
        Task<Guid> UpdatePartyAsync(PartyDto party);
    }
}