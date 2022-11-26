using StarBucks.Domain.Commons;
namespace StarBucks.Domain.Entities
{
    public class Address : Auditable
    {
        public string City { get; set; }
        public string FullAddres { get; set; }
    }
}