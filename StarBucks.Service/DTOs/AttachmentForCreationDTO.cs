using System.IO;

namespace StarBucks.Service.DTOs
{
    public class AttachmentForCreationDTO
    {
        public string Name { get; set; }

        public Stream Stream { get; set; }
    }
}
