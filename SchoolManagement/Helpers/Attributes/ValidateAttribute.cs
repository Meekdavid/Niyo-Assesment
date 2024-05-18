using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SchoolManagement.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ValidateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(value?.ToString()))
                {
                    return new ValidationResult($"The {validationContext.DisplayName} Field is Required");
                }
                string? value1 = value is object ? value.ToString() : "";

                if (!string.IsNullOrEmpty(value1))
                {
                    //XML/SQL Injection Checks
                    string pattern = @"[<>&'$=]|(\bOR\b)";
                    if (Regex.IsMatch(value1, pattern, RegexOptions.IgnoreCase))
                    {
                        return new ValidationResult($"The {validationContext.DisplayName} Contains Special Character");
                    }

                    value1 = Regex.Replace(value1, "<.*?>", string.Empty);
                }

                validationContext.ObjectType.GetProperty(validationContext.MemberName).SetValue(validationContext.ObjectInstance, value1, null);
            }
            catch (Exception ex)
            {
                return new ValidationResult("An Unknown Error Occurred, Please Try Again!");
            }
            return ValidationResult.Success;
        }
    }
}
