using StarBucks.Domain.Commons;
using StarBucks.Domain.Enums;

namespace StarBucks.Domain.Entities
{
    public class User : Auditable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public UserRole Role { get; set; }
    }
}
