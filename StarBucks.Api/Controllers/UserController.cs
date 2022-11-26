using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarBucks.Api.Helpers;
using StarBucks.Domain.Configurations;
using StarBucks.Domain.Enums;
using StarBucks.Service.DTOs;
using StarBucks.Service.Helpers;
using StarBucks.Service.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace StarBucks.Api.Controllers
{
    [ApiController(), Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPut, Authorize(Roles = CustomRoles.USER_ROLE)]
        public async ValueTask<IActionResult> UpdateUser([Required] string login, [Required] string password, [FromQuery] UserForUpdateDTO userForUpdateDTO)
            => Ok(await userService.UpdateAsync(login, password, userForUpdateDTO));

        [HttpPatch("{id}"), Authorize(Roles = CustomRoles.ADMIN_ROLE)]
        public async ValueTask<IActionResult> ChangeRoleAsync(int id, UserRole userRole)
            => Ok(await userService.ChangeRoleAsync(id, userRole));

        [HttpPatch("Password"), Authorize(Roles = CustomRoles.USER_ROLE)]
        public async ValueTask<IActionResult> ChangePasswordAsync(string oldPassword, string newPassword)
            => Ok(await userService.ChangePasswordAsync(oldPassword, newPassword));

        [HttpGet, Authorize(Roles = CustomRoles.ADMIN_ROLE)]
        public async ValueTask<IActionResult> GetAll([FromQuery] PaginationParams @params)
            => Ok(await userService.GetAllAsync(@params));

        [HttpGet("{id}/Admin"), Authorize(Roles = CustomRoles.ADMIN_ROLE)]
        public async ValueTask<IActionResult> GetAsync([FromRoute] int id)
            => Ok(await userService.GetAsync(u => u.Id == id));

        [HttpGet("User"), Authorize(Roles = CustomRoles.USER_ROLE)]
        public async ValueTask<IActionResult> GetAsync()
            => Ok(await userService.GetAsync(u => u.Id == HttpContextHelper.UserId));
    }
}
