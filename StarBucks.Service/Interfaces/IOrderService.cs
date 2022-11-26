using StarBucks.Domain.Configurations;
using StarBucks.Domain.Entities;
using StarBucks.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StarBucks.Service.Interfaces
{
    public interface IOrderService
    {
        ValueTask<Orders> CreateAsync(OrdersForCreationDTO orderForCreationDTO);

        ValueTask<Orders> UpdateAsync(int id, OrdersForCreationDTO orderForCreationDTO);

        ValueTask<bool> DeleteAsync(Expression<Func<Orders, bool>> expression);

        ValueTask<IEnumerable<Orders>> GetAllAsync(
            PaginationParams @params = null,
            Expression<Func<Orders, bool>> expression = null);

        ValueTask<Orders> GetAsync(Expression<Func<Orders, bool>> expression);
    }
}