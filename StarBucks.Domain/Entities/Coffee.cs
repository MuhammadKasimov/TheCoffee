
using StarBucks.Domain.Commons;

namespace StarBucks.Domain.Entities
{
    public class Coffee : Auditable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
    }
}
