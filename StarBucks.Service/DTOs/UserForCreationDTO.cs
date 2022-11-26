using StarBucks.Service.Attributes;
using System.ComponentModel.DataAnnotations;

namespace StarBucks.Service.DTOs
{
    public class UserForCreationDTO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [UserLogin, Required]
        public string Username { get; set; }

        [UserPassword, Required]
        public string Password { get; set; }

        [PhoneNumber, Required]
        public string PhoneNumber { get; set; }
        public AddressForCreationDTO Address { get; set; }
    }
}