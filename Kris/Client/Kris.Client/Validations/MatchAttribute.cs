using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Kris.Client.Validations;

// Source: https://stackoverflow.com/a/69599711
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public sealed class MatchAttribute : ValidationAttribute
{
    private const string _defaultErrorMessage = "Attributes do not match";

    public string OriginalProperty { get; }

    public MatchAttribute(string originalProperty)
        : base(_defaultErrorMessage)
    {
        OriginalProperty = originalProperty;
    }

    public MatchAttribute(string originalProperty, string errorMessage)
        : base(errorMessage)
    {
        OriginalProperty = originalProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var properties = TypeDescriptor.GetProperties(validationContext.ObjectInstance);
        var originalProperty = properties.Find(OriginalProperty, false);
        var originalValue = originalProperty.GetValue(validationContext.ObjectInstance);
        var confirmProperty = properties.Find(validationContext.MemberName, false);
        var confirmValue = confirmProperty.GetValue(validationContext.ObjectInstance);

        if (originalValue == null)
        {
            if (confirmValue == null)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage, new string[] { validationContext.MemberName });
        }

        if (originalValue.Equals(confirmValue))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage, new string[] { validationContext.MemberName });
    }
}
