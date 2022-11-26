using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarBucks.Api.Helpers;
using StarBucks.Domain.Configurations;
using StarBucks.Service.DTOs;
using StarBucks.Service.Helpers;
using StarBucks.Service.Interfaces;
using System.Threading.Tasks;

namespace StarBucks.Api.Controllers
{
    [ApiController, Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost, Authorize(Roles = CustomRoles.USER_ROLE)]
        public async ValueTask<IActionResult> CreateAsync(OrdersForCreationDTO ordersForCreationDTO)
            => Ok(await orderService.CreateAsync(ordersForCreationDTO));

        [HttpDelete("{id}"), Authorize(Roles = CustomRoles.USER_ROLE)]
        public async ValueTask<IActionResult> DeleteAsync([FromRoute] int id)
            => Ok(await orderService.DeleteAsync(o => o.Id == id));

        [HttpPut("{id}"), Authorize(Roles = CustomRoles.USER_ROLE)]
        public async ValueTask<IActionResult> UpdateAsync([FromRoute] int id, OrdersForCreationDTO ordersForCreationDTO)
            => Ok(await orderService.UpdateAsync(id, ordersForCreationDTO));

        [HttpGet("{id}"), Authorize(Roles = CustomRoles.USER_ROLE)]
        public async ValueTask<IActionResult> GetAsync([FromRoute] int id)
            => Ok(await orderService.GetAsync(o => o.Id == id));

        [HttpGet, Authorize(Roles = CustomRoles.USER_ROLE)]
        public async ValueTask<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
            => Ok(await orderService.GetAllAsync(@params, o => o.UserId == HttpContextHelper.UserId));

    }
}
