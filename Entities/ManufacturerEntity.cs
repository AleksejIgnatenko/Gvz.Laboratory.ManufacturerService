﻿namespace Gvz.Laboratory.ManufacturerService.Entities
{
    public class ManufacturerEntity
    {
        public Guid Id { get; set; }
        public string ManufacturerName { get; set; } = string.Empty;
        public DateTime DateCreate { get; set; }
    }
}