using StarBucks.Domain.Commons;

namespace StarBucks.Domain.Entities
{
    public class Orders : Auditable
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int CoffeeId { get; set; }
        public Coffee Coffee { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
    }
}
