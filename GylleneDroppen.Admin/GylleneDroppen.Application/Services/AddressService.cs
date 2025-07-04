using GylleneDroppen.Application.Dtos.Address;
using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Application.Interfaces.Services;
using GylleneDroppen.Core.Entities;
using Microsoft.Extensions.Logging;

namespace GylleneDroppen.Application.Services;

public class AddressService(
    IAddressRepository addressRepository,
    ICurrentUserService currentUserService,
    ILogger<AddressService> logger) : IAddressService
{
    public async Task<List<AddressResponseDto>> GetAllAddressesAsync()
    {
        var addresses = await addressRepository.GetAllAddressesAsync();
        return addresses.Select(MapToResponseDto).ToList();
    }

    public async Task<List<AddressResponseDto>> GetActiveAddressesAsync()
    {
        var addresses = await addressRepository.GetActiveAddressesAsync();
        return addresses.Select(MapToResponseDto).ToList();
    }

    public async Task<AddressResponseDto?> GetAddressByIdAsync(Guid id)
    {
        var address = await addressRepository.GetByIdAsync(id);
        return address == null ? null : MapToResponseDto(address);
    }

    public async Task<AddressResponseDto> CreateAddressAsync(CreateAddressRequestDto dto)
    {
        var currentUserId = currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad för att skapa adresser.");

        var address = new Address
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            StreetAddress = dto.StreetAddress,
            City = dto.City,
            PostalCode = dto.PostalCode,
            Description = dto.Description,
            IsActive = dto.IsActive,
            CreatedDate = DateTime.UtcNow,
            CreatedByUserId = currentUserId
        };

        await addressRepository.AddAsync(address);
        await addressRepository.SaveChangesAsync();

        logger.LogInformation("Address '{AddressName}' created by user {UserId}", dto.Name, currentUserId);

        // Return the created address with navigation properties loaded
        var created = await addressRepository.GetByIdAsync(address.Id);
        return MapToResponseDto(created!);
    }

    public async Task<AddressResponseDto> UpdateAddressAsync(UpdateAddressRequestDto dto)
    {
        var currentUserId = currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad för att uppdatera adresser.");

        var address = await addressRepository.GetByIdAsync(dto.Id);
        if (address == null)
            throw new InvalidOperationException("Adressen hittades inte.");

        address.Name = dto.Name;
        address.StreetAddress = dto.StreetAddress;
        address.City = dto.City;
        address.PostalCode = dto.PostalCode;
        address.Description = dto.Description;
        address.IsActive = dto.IsActive;
        address.UpdatedDate = DateTime.UtcNow;
        address.UpdatedByUserId = currentUserId;

        addressRepository.Update(address);
        await addressRepository.SaveChangesAsync();

        logger.LogInformation("Address '{AddressName}' updated by user {UserId}", dto.Name, currentUserId);

        // Return the updated address with navigation properties loaded
        var updated = await addressRepository.GetByIdAsync(address.Id);
        return MapToResponseDto(updated!);
    }

    public async Task<bool> DeleteAddressAsync(Guid id)
    {
        var currentUserId = currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad för att ta bort adresser.");

        var address = await addressRepository.GetByIdAsync(id);
        if (address == null)
            return false;

        addressRepository.Delete(address);
        await addressRepository.SaveChangesAsync();

        logger.LogInformation("Address '{AddressName}' deleted by user {UserId}", address.Name, currentUserId);

        return true;
    }

    private static AddressResponseDto MapToResponseDto(Address address)
    {
        return new AddressResponseDto
        {
            Id = address.Id,
            Name = address.Name,
            StreetAddress = address.StreetAddress,
            City = address.City,
            PostalCode = address.PostalCode,
            Description = address.Description,
            IsActive = address.IsActive,
            CreatedDate = address.CreatedDate,
            UpdatedDate = address.UpdatedDate,
            CreatedByUserName = address.CreatedByUser?.Email ?? "Okänd",
            UpdatedByUserName = address.UpdatedByUser?.Email
        };
    }
}