using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StarBucks.Service.Attributes
{
    public class UserPassword : ValidationAttribute
    {
        public override bool IsValid(object value)
            => value is string password &&
                password.Length >= 8 &&
                    password.Any(c => char.IsDigit(c)) &&
                        password.Any(c => char.IsLetter(c));
    }
}
