using Gvz.Laboratory.ManufacturerService.Abstractions;
using Gvz.Laboratory.ManufacturerService.Contracts;
using Gvz.Laboratory.ManufacturerService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gvz.Laboratory.ManufacturerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartyController : ControllerBase
    {
        private readonly IPartyService _partyService;

        public PartyController(IPartyService partyService)
        {
            _partyService = partyService;
        }

        [HttpGet]
        [Route("getManufacturerPartiesForPage")]
        [Authorize]
        public async Task<ActionResult> GetManufacturerPartiesForPageAsync(Guid manufacturerId, int pageNumber)
        {
            var (parties, numberParties) = await _partyService.GetManufacturerPartiesForPageAsync(manufacturerId, pageNumber);

            var response = parties.Select(p => new GetPartiesResponse(p.Id,
                p.BatchNumber,
                p.DateOfManufacture,
                p.ProductName,
                p.SupplierName,
                p.Manufacturer.ManufacturerName,
                p.BatchSize,
                p.SampleSize,
                p.TTN,
                p.DocumentOnQualityAndSafety,
                p.TestReport,
                p.DateOfManufacture,
                p.ExpirationDate,
                p.Packaging,
                p.Marking,
                p.Result,
                p.Surname,
                p.Note)).ToList();


            var responseWrapper = new GetPartiesForPageResponseWrapper(response, numberParties);

            return Ok(responseWrapper);
        }
    }
}
