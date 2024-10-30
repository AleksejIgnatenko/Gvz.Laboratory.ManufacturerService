namespace Gvz.Laboratory.ManufacturerService.Contracts
{
    public record GetManufacturersForPageResponseWrapper(
        List<GetManufacturersForPageResponse> Products,
        int numberManufacturers
        );
}
