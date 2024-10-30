using FluentValidation;
using Gvz.Laboratory.ManufacturerService.Models;

namespace Gvz.Laboratory.ManufacturerService.Validations
{
    public class ManufacturerValidation : AbstractValidator<ManufacturerModel>
    {
        public ManufacturerValidation()
        {
            RuleFor(x => x.ManufacturerName)
                    .NotEmpty().WithMessage("Название производителя не может быть пустым");
        }
    }
}
