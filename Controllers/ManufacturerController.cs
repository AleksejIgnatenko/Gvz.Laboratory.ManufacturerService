using Gvz.Laboratory.ManufacturerService.Abstractions;
using Gvz.Laboratory.ManufacturerService.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Gvz.Laboratory.ManufacturerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManufacturerController : ControllerBase
    {
        private readonly IManufacturerService _manufacturerService;

        public ManufacturerController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateManufacturerAsync([FromBody] CreateManufacturerRequest createManufacturerRequest)
        {
            var id = await _manufacturerService.CreateManufacturerAsync(Guid.NewGuid(),
                                                        createManufacturerRequest.ManufacturerName);
            return Ok();
        }

        [HttpGet]
        [Route("getManufacturersAsync")]
        public async Task<ActionResult> GetManufacturersAsync()
        {
            var manufacturers = await _manufacturerService.GetManufacturersAsync();

            var response = manufacturers.Select(m => new GetManufacturersResponse(m.Id, m.ManufacturerName)).ToList();

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult> GetManufacturerForPageAsync(int pageNumber)
        {
            var (manufacturers, numberManufacturers) = await _manufacturerService.GetManufacturersForPageAsync(pageNumber);

            var response = manufacturers.Select(m => new GetManufacturersForPageResponse(m.Id, m.ManufacturerName)).ToList();

            var responseWrapper = new GetManufacturersForPageResponseWrapper(response, numberManufacturers);

            return Ok(responseWrapper);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateManufacturerAsync(Guid id, [FromBody] UpdateManufacturerRequest updateManufacturerRequest)
        {
            await _manufacturerService.UpdateManufacturerAsync(id, updateManufacturerRequest.ManufacturerName);

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteManufacturerAsync([FromBody] List<Guid> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("No supplier IDs provided.");
            }

            await _manufacturerService.DeleteManufacturersAsync(ids);

            return Ok();
        }
    }
}
