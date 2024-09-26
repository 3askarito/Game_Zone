using Microsoft.Extensions.Options;

namespace GameZone.Attributes
{
    public class MaxSizeAttribute : ValidationAttribute
    {
        private readonly int maxFileSize;

        public MaxSizeAttribute(int maxFileSize)
        {
            this.maxFileSize = maxFileSize;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file is not null)
            {
                if (file.Length > maxFileSize)
                {
                    return new ValidationResult($"Maximum allowed size is {maxFileSize} bytes!!!");
                }
            }
            return ValidationResult.Success;
        }
    }
}
