using GylleneDroppen.Core.Entities;

namespace GylleneDroppen.Application.Interfaces.Repositories;

public interface IUserRepository : IRepository<ApplicationUser>
{
    Task<ApplicationUser?> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
}