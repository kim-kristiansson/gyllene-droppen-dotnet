using GylleneDroppen.Admin.Api.Models;

namespace GylleneDroppen.Admin.Api.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}