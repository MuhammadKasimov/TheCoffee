using StarBucks.Domain.Configurations;
using StarBucks.Domain.Entities;
using StarBucks.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StarBucks.Service.Interfaces
{
    public interface ICoffeeService
    {
        ValueTask<Coffee> CreateAsync(CoffeeForCreationDTO coffeeForCreationDTO);

        ValueTask<Coffee> UpdateAsync(int id, CoffeeForCreationDTO coffeeForCreationDTO);

        ValueTask<bool> DeleteAsync(Expression<Func<Coffee, bool>> expression);

        ValueTask<IEnumerable<Coffee>> GetAllAsync(
            PaginationParams @params = null,
            Expression<Func<Coffee, bool>> expression = null);

        ValueTask<Coffee> GetAsync(Expression<Func<Coffee, bool>> expression);

    }
}
