using Confluent.Kafka;
using Gvz.Laboratory.ManufacturerService.Abstractions;
using Gvz.Laboratory.ManufacturerService.Dto;
using System.Text.Json;

namespace Gvz.Laboratory.ManufacturerService.Kafka
{
    public class ManufacturerKafkaProducer : IManufacturerKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;

        public ManufacturerKafkaProducer(IProducer<Null, string> producer)
        {
            _producer = producer;
        }

        public async Task SendManufacturerToKafkaAsync(ManufacturerDto manufacturer, string topic)
        {
            var serializedManufacturer = JsonSerializer.Serialize(manufacturer);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = serializedManufacturer });
        }

        public async Task SendManufacturerToKafkaAsync(List<Guid> manufacturersIds, string topic)
        {
            var serializedManufacturer = JsonSerializer.Serialize(manufacturersIds);
            Console.WriteLine(serializedManufacturer);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = serializedManufacturer });
        }
    }
}
