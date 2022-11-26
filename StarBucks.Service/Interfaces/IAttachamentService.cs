using StarBucks.Domain.Entities;
using StarBucks.Service.DTOs;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StarBucks.Service.Interfaces
{
    public interface IAttachmentService
    {
        ValueTask<Attachment> UploadAsync(AttachmentForCreationDTO dto);
        ValueTask<Attachment> UpdateAsync(int id, Stream stream);
        ValueTask<bool> DeleteAsync(Expression<Func<Attachment, bool>> expression);
        ValueTask<Attachment> GetAsync(Expression<Func<Attachment, bool>> expression);
        ValueTask<Attachment> CreateAsync(string fileName, string filePath);
    }
}
