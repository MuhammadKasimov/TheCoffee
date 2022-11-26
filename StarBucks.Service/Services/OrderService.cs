using Mapster;
using Microsoft.EntityFrameworkCore;
using StarBucks.Data.IRepositories;
using StarBucks.Domain.Configurations;
using StarBucks.Domain.Entities;
using StarBucks.Service.DTOs;
using StarBucks.Service.Exceptions;
using StarBucks.Service.Extensions;
using StarBucks.Service.Helpers;
using StarBucks.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StarBucks.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async ValueTask<Orders> CreateAsync(OrdersForCreationDTO orderForCreationDTO)
        {
            orderForCreationDTO.UserId = HttpContextHelper.UserId ?? throw new StarBucksException(404, "User not found");

            var coffee = await unitOfWork.Coffees.GetAsync(u => u.Id == orderForCreationDTO.CoffeeId);

            if (coffee is null)
                throw new StarBucksException(404, "coffee not found");

            var order = await unitOfWork.Orders.CreateAsync(orderForCreationDTO.Adapt<Orders>());
            await unitOfWork.SaveChangesAsync();
            return order;
        }

        public async ValueTask<bool> DeleteAsync(Expression<Func<Orders, bool>> expression)
        {
            var isDeleted = await unitOfWork.Orders.DeleteAsync(expression);

            await unitOfWork.SaveChangesAsync();

            return isDeleted ? true : throw new StarBucksException(404, "Order not found");
        }

        public async ValueTask<IEnumerable<Orders>> GetAllAsync(PaginationParams @params = null, Expression<Func<Orders, bool>> expression = null)
        {
            var orders = unitOfWork.Orders.GetAll(expression: expression, new string[] { "User", "Coffee" }, false);

            return await orders.ToPagedList(@params).ToListAsync();
        }

        public async ValueTask<Orders> GetAsync(Expression<Func<Orders, bool>> expression)
        {
            var order = await unitOfWork.Orders.GetAsync(expression, new string[] { "User", "Coffee" });
            return order ?? throw new StarBucksException(404, "Order not foud");
        }

        public async ValueTask<Orders> UpdateAsync(int id, OrdersForCreationDTO orderForCreationDTO)
        {
            var order = await GetAsync(o => o.Id == id);

            orderForCreationDTO.UserId = HttpContextHelper.UserId ?? throw new StarBucksException(404, "User not found");

            var coffee = await unitOfWork.Coffees.GetAsync(u => u.Id == orderForCreationDTO.CoffeeId);

            if (coffee is null)
                throw new StarBucksException(404, "coffee not found");

            order.UpdatedAt = DateTime.UtcNow;

            order = unitOfWork.Orders.Update(orderForCreationDTO.Adapt(order));


            await unitOfWork.SaveChangesAsync();

            return order;
        }
    }
}
