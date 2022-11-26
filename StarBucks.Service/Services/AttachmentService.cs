using StarBucks.Data.IRepositories;
using StarBucks.Domain.Entities;
using StarBucks.Service.DTOs;
using StarBucks.Service.Exceptions;
using StarBucks.Service.Helpers;
using StarBucks.Service.Interfaces;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StarBucks.Service.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IUnitOfWork unitOfWork;

        public AttachmentService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async ValueTask<Attachment> CreateAsync(string fileName, string filePath)
        {
            var file = new Attachment()
            {
                Name = fileName,
                Path = filePath
            };

            file = await unitOfWork.Attachments.CreateAsync(file);
            await unitOfWork.SaveChangesAsync();

            return file;
        }

        public async ValueTask<bool> DeleteAsync(Expression<Func<Attachment, bool>> expression)
        {
            var file = await unitOfWork.Attachments.GetAsync(expression);

            if (file is null)
                throw new StarBucksException(404, "Attachment not found");

            // delete file from wwwroot
            string fullPath = Path.Combine(EnvironmentHelper.WebRootPath, file.Path);

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            // datele database information
            FileHelper.Remove(file.Path);

            await unitOfWork.Attachments.DeleteAsync(expression);
            await unitOfWork.SaveChangesAsync();

            return true;
        }

        public async ValueTask<Attachment> UploadAsync(AttachmentForCreationDTO dto)
        {
            // genarate file destination
            string fileName = Guid.NewGuid().ToString("N") + "-" + dto.Name;
            string filePath = Path.Combine(EnvironmentHelper.AttachmentPath, fileName);

            if (!Directory.Exists(EnvironmentHelper.AttachmentPath))
                Directory.CreateDirectory(EnvironmentHelper.AttachmentPath);

            // copy image to the destination as stream
            FileStream fileStream = File.OpenWrite(filePath);
            await dto.Stream.CopyToAsync(fileStream);

            // clear
            await fileStream.FlushAsync();
            fileStream.Close();

            return await CreateAsync(fileName, Path.Combine(EnvironmentHelper.FilePath, fileName));
        }

        public async ValueTask<Attachment> GetAsync(Expression<Func<Attachment, bool>> expression)
        {
            var existAttachement = await unitOfWork.Attachments.GetAsync(expression);

            return existAttachement ?? throw new StarBucksException(404, "Attachment not found.");
        }

        public async ValueTask<Attachment> UpdateAsync(int id, Stream stream)
        {
            var existAttachment = await unitOfWork.Attachments.GetAsync(a => a.Id == id, null);

            if (existAttachment is null)
                throw new StarBucksException(404, "Attachment not found.");

            string fileName = existAttachment.Path;
            string filePath = Path.Combine(EnvironmentHelper.WebRootPath, fileName);

            // copy image to the destination as stream
            FileStream fileStream = File.OpenWrite(filePath);
            await stream.CopyToAsync(fileStream);

            // clear
            await fileStream.FlushAsync();
            fileStream.Close();

            await unitOfWork.SaveChangesAsync();

            return existAttachment;
        }
    }
}