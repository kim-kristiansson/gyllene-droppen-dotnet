using GylleneDroppen.Core.Entities;
using GylleneDroppen.Core.Enums;

namespace GylleneDroppen.Core.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<List<User>> GetUsersByRoleAsync(RoleType role);
}