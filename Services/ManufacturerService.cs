using Gvz.Laboratory.ManufacturerService.Abstractions;
using Gvz.Laboratory.ManufacturerService.Dto;
using Gvz.Laboratory.ManufacturerService.Exceptions;
using Gvz.Laboratory.ManufacturerService.Models;
using OfficeOpenXml;

namespace Gvz.Laboratory.ManufacturerService.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IManufacturerKafkaProducer _producer;

        public ManufacturerService(IManufacturerRepository manufacturerRepository, IManufacturerKafkaProducer producer)
        {
            _manufacturerRepository = manufacturerRepository;
            _producer = producer;
        }

        public async Task<Guid> CreateManufacturerAsync(Guid id, string manufacturerName)
        {
            var (errors, manufacturer) = ManufacturerModel.Create(id, manufacturerName);
            if (errors.Count > 0)
            {
                throw new ManufacturerValidationException(errors);
            }

            var manufacturerId = await _manufacturerRepository.CreateManufacturerAsync(manufacturer);

            ManufacturerDto manufacturerDto = new ManufacturerDto
            {
                Id = id,
                ManufacturerName = manufacturerName
            };

            await _producer.SendManufacturerToKafkaAsync(manufacturerDto, "add-manufacturer-topic");

            return manufacturerId;
        }

        public async Task<List<ManufacturerModel>> GetManufacturersAsync()
        {
            return await _manufacturerRepository.GetManufacturersAsync();
        }

        public async Task<(List<ManufacturerModel> manufacturers, int numberManufacturers)> GetManufacturersForPageAsync(int pageNumber)
        {
            return await _manufacturerRepository.GetManufacturersForPageAsync(pageNumber);
        }

        public async Task<(List<ManufacturerModel> manufacturers, int numberManufacturers)> SearchManufacturersAsync(string searchQuery, int pageNumber)
        {
            return await _manufacturerRepository.SearchManufacturersAsync(searchQuery, pageNumber);
        }

        public async Task<MemoryStream> ExportManufacturersToExcelAsync()
        {
            var manufacturers = await _manufacturerRepository.GetManufacturersAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Manufacturers");

                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Название";

                for (int i = 0; i < manufacturers.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = manufacturers[i].Id;
                    worksheet.Cells[i + 2, 2].Value = manufacturers[i].ManufacturerName;
                }

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                await package.SaveAsAsync(stream);

                stream.Position = 0; // Сбрасываем поток
                return stream; 
            }
        }

        public async Task<Guid> UpdateManufacturerAsync(Guid id, string manufacturerName)
        {
            var (errors, manufacturer) = ManufacturerModel.Create(id, manufacturerName);
            if (errors.Count > 0)
            {
                throw new ManufacturerValidationException(errors);
            }

            var manufacturerId = await _manufacturerRepository.UpdateManufacturerAsync(manufacturer);

            ManufacturerDto manufacturerDto = new ManufacturerDto
            {
                Id = id,
                ManufacturerName = manufacturerName
            };

            await _producer.SendManufacturerToKafkaAsync(manufacturerDto, "update-manufacturer-topic");

            return manufacturerId;
        }

        public async Task DeleteManufacturersAsync(List<Guid> ids)
        {
            await _manufacturerRepository.DeleteManufacturersAsync(ids);

            await _producer.SendManufacturerToKafkaAsync(ids, "delete-manufacturer-topic");
        }
    }
}
