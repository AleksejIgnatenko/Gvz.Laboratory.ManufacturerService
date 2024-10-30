namespace Gvz.Laboratory.ManufacturerService.Exceptions
{
    public class ManufacturerValidationException : Exception
    {
        public Dictionary<string, string> Errors { get; set; }

        public ManufacturerValidationException(Dictionary<string, string> errors)
        {
            Errors = errors;
        }
    }
}
