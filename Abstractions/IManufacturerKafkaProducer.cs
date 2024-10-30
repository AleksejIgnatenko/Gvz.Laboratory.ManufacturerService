﻿using Gvz.Laboratory.ManufacturerService.Dto;

namespace Gvz.Laboratory.ManufacturerService.Abstractions
{
    public interface IManufacturerKafkaProducer
    {
        Task SendManufacturerToKafkaAsync(ManufacturerDto manufacturer, string topic);
    }
}