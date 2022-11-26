using System.Text.Json.Serialization;

namespace StarBucks.Service.DTOs
{
    public class OrdersForCreationDTO
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public int CoffeeId { get; set; }
        public AddressForCreationDTO Address { get; set; }
    }
}
