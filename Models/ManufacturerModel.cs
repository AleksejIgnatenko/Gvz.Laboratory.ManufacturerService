using FluentValidation.Results;
using Gvz.Laboratory.ManufacturerService.Validations;

namespace Gvz.Laboratory.ManufacturerService.Models
{
    public class ManufacturerModel
    {
        public Guid Id { get; }
        public string ManufacturerName { get; } = string.Empty;

        public ManufacturerModel(Guid id, string manufacturerName)
        {
            Id = id;
            ManufacturerName = manufacturerName;
        }

        public static (Dictionary<string, string> errors, ManufacturerModel manufacturer) Create(Guid id, string manufacturerName, bool useValidation = true)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            ManufacturerModel manufacturer = new ManufacturerModel(id, manufacturerName);
            if(!useValidation) { return (errors, manufacturer); }

            ManufacturerValidation manufacturerValidation = new ManufacturerValidation();
            ValidationResult validationResult = manufacturerValidation.Validate(manufacturer);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    errors[failure.PropertyName] = failure.ErrorMessage;
                }
            }

            return (errors, manufacturer);
        }
    }
}
