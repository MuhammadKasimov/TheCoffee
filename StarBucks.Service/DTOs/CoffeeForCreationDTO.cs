namespace StarBucks.Service.DTOs
{
    public class CoffeeForCreationDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int? AttachmentId { get; set; }
    }
}
