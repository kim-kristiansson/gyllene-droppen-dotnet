using GylleneDroppen.Domain.Entities;
using GylleneDroppen.Domain.Enums;

namespace GylleneDroppen.Application.Interfaces.Shared.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<List<User>> GetUsersByRoleAsync(RoleType role);
}