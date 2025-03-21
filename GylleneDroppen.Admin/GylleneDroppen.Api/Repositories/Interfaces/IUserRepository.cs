using GylleneDroppen.Api.Enums;
using GylleneDroppen.Api.Models;
using StackExchange.Redis;

namespace GylleneDroppen.Api.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<List<User>> GetUsersByRoleAsync(RoleType role);
}