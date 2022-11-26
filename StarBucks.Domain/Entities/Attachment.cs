using StarBucks.Domain.Commons;

namespace StarBucks.Domain.Entities
{
    public class Attachment : Auditable
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}