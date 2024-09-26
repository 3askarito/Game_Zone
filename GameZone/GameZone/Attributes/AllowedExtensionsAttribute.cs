using GameZone.Settings;
using Microsoft.Extensions.Options;

namespace GameZone.Attributes
{
    public class AllowedExtensionsAttribute(string options) : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if(file is not null)
            {
                var extionsions = Path.GetExtension(file.FileName);
                if (!options.Split(',').Contains(extionsions, StringComparer.OrdinalIgnoreCase))
                {
                    return new ValidationResult($"Only {options} extissins are Allowed!!!");
                }
            }
            return ValidationResult.Success;
        }
    }
}
