using Gvz.Laboratory.ManufacturerService.Abstractions;
using Gvz.Laboratory.ManufacturerService.Contracts;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin,Manager,Worker")]
        public async Task<ActionResult> CreateManufacturerAsync([FromBody] CreateManufacturerRequest createManufacturerRequest)
        {
            var id = await _manufacturerService.CreateManufacturerAsync(Guid.NewGuid(),
                                                        createManufacturerRequest.ManufacturerName);
            return Ok();
        }

        [HttpGet]
        [Route("getManufacturersAsync")]
        [Authorize]
        public async Task<ActionResult> GetManufacturersAsync()
        {
            var manufacturers = await _manufacturerService.GetManufacturersAsync();

            var response = manufacturers.Select(m => new GetManufacturersResponse(m.Id, m.ManufacturerName)).ToList();

            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetManufacturerForPageAsync(int pageNumber)
        {
            var (manufacturers, numberManufacturers) = await _manufacturerService.GetManufacturersForPageAsync(pageNumber);

            var response = manufacturers.Select(m => new GetManufacturersForPageResponse(m.Id, m.ManufacturerName)).ToList();

            var responseWrapper = new GetManufacturersForPageResponseWrapper(response, numberManufacturers);

            return Ok(responseWrapper);
        }

        [HttpGet]
        [Route("exportManufacturersToExcel")]
        [Authorize]
        public async Task<ActionResult> ExportManufacturersToExcelAsync()
        {
            var stream = await _manufacturerService.ExportManufacturersToExcelAsync();
            var fileName = "Manufacturers.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet]
        [Route("searchManufacturers")]
        [Authorize]
        public async Task<ActionResult> SearchManufacturersAsync(string searchQuery, int pageNumber)
        {
            var (manufacturers, numberManufacturers) = await _manufacturerService.SearchManufacturersAsync(searchQuery, pageNumber);

            var response = manufacturers.Select(m => new GetManufacturersForPageResponse(m.Id, m.ManufacturerName)).ToList();

            var responseWrapper = new GetManufacturersForPageResponseWrapper(response, numberManufacturers);

            return Ok(responseWrapper);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Manager,Worker")]
        public async Task<ActionResult> UpdateManufacturerAsync(Guid id, [FromBody] UpdateManufacturerRequest updateManufacturerRequest)
        {
            await _manufacturerService.UpdateManufacturerAsync(id, updateManufacturerRequest.ManufacturerName);

            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,Manager,Worker")]
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
