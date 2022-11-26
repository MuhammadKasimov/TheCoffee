using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarBucks.Api.Extensions;
using StarBucks.Api.Helpers;
using StarBucks.Domain.Configurations;
using StarBucks.Domain.Entities;
using StarBucks.Service.Attributes;
using StarBucks.Service.DTOs;
using StarBucks.Service.Interfaces;
using System.Threading.Tasks;

namespace StarBucks.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoffeeController : ControllerBase
    {
        private readonly ICoffeeService coffeeService;
        private readonly IAttachmentService attachmentService;
        public CoffeeController(ICoffeeService coffeeService, IAttachmentService attachmentService)
        {
            this.coffeeService = coffeeService;
            this.attachmentService = attachmentService;
        }

        [HttpGet]
        public async ValueTask<IActionResult> GetAll([FromQuery] PaginationParams @params)
        {
            return Ok(await coffeeService.GetAllAsync(@params));
        }

        [HttpGet("{id}")]
        public async ValueTask<IActionResult> GetAsync([FromRoute] int id)
            => Ok(await coffeeService.GetAsync(c => c.Id == id));

        [HttpPost, Authorize(Roles = CustomRoles.ADMIN_ROLE)]
        public async ValueTask<IActionResult> CreateAsync(string name, string description, decimal price, [FormFileAttributes] IFormFile formFile)
        {
            Attachment attachment = await attachmentService.UploadAsync(formFile.ToAttachmentOrDefault());
            var coffeeForCreationDTO = new CoffeeForCreationDTO
            {
                Name = name,
                Description = description,
                Price = price,
                AttachmentId = attachment.Id
            };

            return Ok(await coffeeService.CreateAsync(coffeeForCreationDTO));
        }

        [HttpDelete("{Id}"), Authorize(Roles = CustomRoles.ADMIN_ROLE)]
        public async ValueTask<IActionResult> DeleteAsync([FromRoute] int id)
            => Ok(await coffeeService.DeleteAsync(c => c.Id == id));

        [HttpPut("{id}"), Authorize(Roles = CustomRoles.ADMIN_ROLE)]
        public async ValueTask<IActionResult> UpdateAsync(
            [FromRoute] int id,
            [FromQuery] CoffeeForCreationDTO coffeeForCreationDTO,
            [FormFileAttributes] IFormFile formFile)
        {
            if (coffeeForCreationDTO.AttachmentId != null && formFile != null)
                await attachmentService.UpdateAsync((int)coffeeForCreationDTO.AttachmentId, formFile.ToAttachmentOrDefault().Stream);

            return Ok(await coffeeService.UpdateAsync(id, coffeeForCreationDTO));
        }
    }
}
