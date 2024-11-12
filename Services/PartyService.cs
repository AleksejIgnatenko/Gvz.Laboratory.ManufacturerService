using Gvz.Laboratory.ManufacturerService.Abstractions;
using Gvz.Laboratory.ManufacturerService.Models;

namespace Gvz.Laboratory.ManufacturerService.Services
{
    public class PartyService : IPartyService
    {
        private readonly IPartyRepository _partyRepository;

        public PartyService(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        public async Task<(List<PartyModel> parties, int numberParties)> GetManufacturerPartiesForPageAsync(Guid manufacturerId, int pageNumber)
        {
            return await _partyRepository.GetManufacturerPartiesForPageAsync(manufacturerId, pageNumber);
        }
    }
}
