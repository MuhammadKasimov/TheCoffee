using Mapster;
using Microsoft.EntityFrameworkCore;
using StarBucks.Data.IRepositories;
using StarBucks.Domain.Configurations;
using StarBucks.Domain.Entities;
using StarBucks.Service.DTOs;
using StarBucks.Service.Exceptions;
using StarBucks.Service.Extensions;
using StarBucks.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StarBucks.Service.Services
{
    public class CoffeeService : ICoffeeService
    {
        private readonly IUnitOfWork unitOfWork;

        public CoffeeService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async ValueTask<Coffee> CreateAsync(CoffeeForCreationDTO coffeeForCreationDTO)
        {
            var alreadyExists = await unitOfWork.Coffees.GetAsync(
                c => c.Name == coffeeForCreationDTO.Name);

            if (alreadyExists != null)
                throw new StarBucksException(400, "Coffee With Such Name Alredy Exists");

            var coffee = await unitOfWork.Coffees.CreateAsync(coffeeForCreationDTO.Adapt<Coffee>());
            await unitOfWork.SaveChangesAsync();

            return coffee;
        }

        public async ValueTask<bool> DeleteAsync(Expression<Func<Coffee, bool>> expression)
        {
            if (!(await unitOfWork.Coffees.DeleteAsync(expression)))
                throw new StarBucksException(404, "Coffee not found");

            await unitOfWork.SaveChangesAsync();

            return true;
        }

        public async ValueTask<IEnumerable<Coffee>> GetAllAsync(PaginationParams @params = null, Expression<Func<Coffee, bool>> expression = null)
        {
            var coffees = unitOfWork.Coffees.GetAll(expression, new string[] { "Attachment" }, false);

            if (@params != null)
                return await coffees.ToPagedList(@params).ToListAsync();

            return await coffees.ToListAsync();
        }

        public async ValueTask<Coffee> GetAsync(Expression<Func<Coffee, bool>> expression) =>
            await unitOfWork.Coffees.GetAsync(expression, new string[] { "Attachment" }) ?? throw new StarBucksException(404, "coffee not found");

        public async ValueTask<Coffee> UpdateAsync(int id, CoffeeForCreationDTO coffeeForCreationDTO)
        {
            var alreadyExists = await unitOfWork.Coffees.GetAsync(
                c => c.Name == coffeeForCreationDTO.Name && c.Id != id);

            if (alreadyExists != null)
                throw new StarBucksException(400, "Coffee With Such Name Alredy Exists");

            var coffee = await GetAsync(c => c.Id == id);

            coffeeForCreationDTO.AttachmentId ??= coffee.AttachmentId;

            coffee.UpdatedAt = DateTime.UtcNow;

            coffee = unitOfWork.Coffees.Update(coffeeForCreationDTO.Adapt(coffee));
            await unitOfWork.SaveChangesAsync();

            return coffee;
        }
    }
}
