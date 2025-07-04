using GylleneDroppen.Application.Dtos.Address;

namespace GylleneDroppen.Application.Interfaces.Services;

public interface IAddressService
{
    Task<List<AddressResponseDto>> GetAllAddressesAsync();
    Task<List<AddressResponseDto>> GetActiveAddressesAsync();
    Task<AddressResponseDto?> GetAddressByIdAsync(Guid id);
    Task<AddressResponseDto> CreateAddressAsync(CreateAddressRequestDto dto);
    Task<AddressResponseDto> UpdateAddressAsync(UpdateAddressRequestDto dto);
    Task<bool> DeleteAddressAsync(Guid id);
}