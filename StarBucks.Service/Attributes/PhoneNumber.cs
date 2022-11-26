using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StarBucks.Service.Attributes
{
    public class PhoneNumber : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return value is string phoneNumber && phoneNumber.Length == 12 &&
                phoneNumber.All(c => char.IsDigit(c)) && phoneNumber.StartsWith("998")
                ? ValidationResult.Success
                : new ValidationResult("Number can only contain numbers and length of it should be 12");
        }
    }
}
