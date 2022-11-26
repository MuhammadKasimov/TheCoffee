using Mapster;
using Microsoft.EntityFrameworkCore;
using StarBucks.Data.IRepositories;
using StarBucks.Domain.Configurations;
using StarBucks.Domain.Entities;
using StarBucks.Domain.Enums;
using StarBucks.Service.Attributes;
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
    public class UserService : IUserService
    {
        public IUnitOfWork unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async ValueTask<UserForViewDTO> CreateAsync(UserForCreationDTO userForCreationDTO)
        {
            var alreadyExistUser = await unitOfWork.Users.GetAsync(u => u.Username == userForCreationDTO.Username);

            if (alreadyExistUser != null)
                throw new StarBucksException(400, "User With Such Username Already Exist");

            userForCreationDTO.Password = userForCreationDTO.Password.Encrypt();

            var user = await unitOfWork.Users.CreateAsync(userForCreationDTO.Adapt<User>());
            user.CreatedAt = DateTime.UtcNow;
            await unitOfWork.SaveChangesAsync();
            return user.Adapt<UserForViewDTO>();
        }

        public async ValueTask<bool> DeleteAsync(Expression<Func<User, bool>> expression)
        {
            var isDeleted = await unitOfWork.Users.DeleteAsync(expression);
            if (!isDeleted)
                throw new StarBucksException(404, "User not found");
            return true;
        }

        public async ValueTask<IEnumerable<UserForViewDTO>> GetAllAsync(PaginationParams @params, Expression<Func<User, bool>> expression = null)
        {
            var users = unitOfWork.Users.GetAll(expression: expression, isTracking: false, includes: new string[] { "Address" });

            return (await users.ToPagedList(@params).ToListAsync()).Adapt<List<UserForViewDTO>>();
        }

        public async ValueTask<UserForViewDTO> GetAsync(Expression<Func<User, bool>> expression)
        {
            var user = await unitOfWork.Users.GetAsync(expression);

            if (user is null)
                throw new StarBucksException(404, "User not found");

            return user.Adapt<UserForViewDTO>();
        }

        public async ValueTask<UserForViewDTO> UpdateAsync(string login, string password, UserForUpdateDTO userForUpdateDTO)
        {
            var alreadyExistUser = await unitOfWork.Users.GetAsync(u => u.Username == userForUpdateDTO.Username && u.Id != HttpContextHelper.UserId);

            if (alreadyExistUser != null)
                throw new StarBucksException(400, "User with such username already exists");

            var existUser = await GetAsync(u => u.Username == login && u.Password == password.Encrypt());

            if (existUser == null)
                throw new StarBucksException(400, "Login or Password is incorrect");

            var user = existUser.Adapt<User>();

            user.UpdatedAt = DateTime.UtcNow;

            user = unitOfWork.Users.Update(user = userForUpdateDTO.Adapt(user));
            await unitOfWork.SaveChangesAsync();
            return user.Adapt<UserForViewDTO>();
        }

        public async ValueTask<bool> ChangeRoleAsync(int id, UserRole userRole)
        {
            var existUser = await unitOfWork.Users.GetAsync(u => u.Id == id);
            if (existUser == null)
                throw new StarBucksException(404, "User not found");

            existUser.Role = userRole;

            unitOfWork.Users.Update(existUser);
            await unitOfWork.SaveChangesAsync();

            return true;
        }

        public async ValueTask<bool> ChangePasswordAsync(string oldPassword, [UserPassword] string newPassword)
        {
            var user = await unitOfWork.Users.GetAsync(u => u.Id == HttpContextHelper.UserId);

            if (user == null)
                throw new StarBucksException(404, "User not found");

            if (user.Password != oldPassword.Encrypt())
            {
                throw new StarBucksException(400, "Password is Incorrect");
            }
            user.Password = newPassword.Encrypt();

            unitOfWork.Users.Update(user);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}