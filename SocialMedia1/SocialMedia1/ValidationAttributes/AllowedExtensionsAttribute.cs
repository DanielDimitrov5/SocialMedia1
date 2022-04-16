using System.ComponentModel.DataAnnotations;

namespace SocialMedia1.ValidationAttributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] extensions;
        public AllowedExtensionsAttribute(string[] extensions)
        {
            this.extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);

                if (!extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            if (!extensions.Any())
            {
                return GlobalConstants.ExtensionAttributeNoAllowedExtensionsError;
            }

            string allowedExtensions =
                extensions.Length > 1 ? string.Join(", ", extensions, 0, extensions.Length - 1) + $" or {extensions[^1]}" : extensions[0];

            //return $"This file extension should be {allowedExtensions}!";
            return string.Format(GlobalConstants.ExtensionAttributeError, allowedExtensions);
        }
    }
}
