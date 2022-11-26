using StarBucks.Domain.Configurations;
using StarBucks.Domain.Entities;
using StarBucks.Domain.Enums;
using StarBucks.Service.Attributes;
using StarBucks.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StarBucks.Service.Interfaces
{
    public interface IUserService
    {
        ValueTask<UserForViewDTO> CreateAsync(UserForCreationDTO userForCreationDTO);

        ValueTask<UserForViewDTO> UpdateAsync(string login, string password, UserForUpdateDTO userForUpdateDTO);

        ValueTask<bool> DeleteAsync(Expression<Func<User, bool>> expression);

        ValueTask<IEnumerable<UserForViewDTO>> GetAllAsync(
            PaginationParams @params,
            Expression<Func<User, bool>> expression = null);

        ValueTask<UserForViewDTO> GetAsync(Expression<Func<User, bool>> expression);

        ValueTask<bool> ChangeRoleAsync(int id, UserRole userRole);

        ValueTask<bool> ChangePasswordAsync(string oldPassword, [UserPassword] string newPassword);
    }
}
