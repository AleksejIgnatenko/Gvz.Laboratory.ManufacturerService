namespace Gvz.Laboratory.ManufacturerService.Contracts
{
    public record GetManufacturersForPageResponseWrapper(
        List<GetManufacturersForPageResponse> Manufacturers,
        int numberManufacturers
        );
}
