using GylleneDroppen.Core.Entities;

namespace GylleneDroppen.Application.Interfaces.Repositories;

public interface IAddressRepository : IRepository<Address>
{
    Task<List<Address>> GetAllAddressesAsync();
    Task<List<Address>> GetActiveAddressesAsync();
    Task<Address?> GetAddressWithDetailsAsync(Guid id);
}