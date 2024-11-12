namespace Gvz.Laboratory.ManufacturerService.Contracts
{
    public record GetPartiesForPageResponseWrapper(
            List<GetPartiesResponse> Parties,
            int numberParties
            );
}
