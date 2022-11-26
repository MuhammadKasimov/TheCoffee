using Microsoft.AspNetCore.Mvc;
using StarBucks.Service.DTOs;
using StarBucks.Service.Interfaces;
using System.Threading.Tasks;

namespace StarBucks.Api.Controllers
{
    [ApiController, Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService authService;
        private readonly IUserService userService;
        public AuthController(IAuthService authService, IUserService userService)
        {
            this.authService = authService;
            this.userService = userService;
        }

        /// <summary>
        /// Authorization
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async ValueTask<IActionResult> Login(UserForLoginDTO dto)
        {
            var token = await authService.GenerateToken(dto.Login, dto.Password);
            return Ok(new
            {
                token
            });
        }

        [HttpPost("register")]
        public async ValueTask<IActionResult> RegisterAsync(UserForCreationDTO userForCreationDTO)
        {
            return Ok(await userService.CreateAsync(userForCreationDTO));
        }
    }
}
