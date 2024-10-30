using Gvz.Laboratory.ManufacturerService.Abstractions;
using Gvz.Laboratory.ManufacturerService.Dto;
using Gvz.Laboratory.ManufacturerService.Exceptions;
using Gvz.Laboratory.ManufacturerService.Models;

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

        public async Task<(List<ManufacturerModel> manufacturers, int numberManufacturers)> GetManufacturersForPageAsync(int pageNumber)
        {
            return await _manufacturerRepository.GetManufacturersForPageAsync(pageNumber);
        }

        public async Task<Guid> UpdateManufacturerAsync(Guid id, string manufacturerName)
        {
            var (errors, manufacturer) = ManufacturerModel.Create(id, manufacturerName);
            if (errors.Count > 0)
            {
                throw new ManufacturerValidationException(errors);
            }

            var manufacturerId = await _manufacturerRepository.UpdateManufacturerAsync(manufacturer);

            //mapping
            //send to kafka

            return manufacturerId;
        }

        public async Task DeleteManufacturersAsync(List<Guid> ids)
        {
            await _manufacturerRepository.DeleteManufacturersAsync(ids);
        }
    }
}
