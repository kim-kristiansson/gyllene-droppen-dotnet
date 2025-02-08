using GylleneDroppen.Api.Models;

namespace GylleneDroppen.Api.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}