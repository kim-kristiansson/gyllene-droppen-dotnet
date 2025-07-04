using GylleneDroppen.Application.Dtos.Department;
using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Application.Interfaces.Services;
using GylleneDroppen.Core.Entities;
using Microsoft.Extensions.Logging;

namespace GylleneDroppen.Application.Services;

public class DepartmentService(
    IDepartmentRepository departmentRepository,
    ICurrentUserService currentUserService,
    ILogger<DepartmentService> logger) : IDepartmentService
{
    public async Task<List<DepartmentResponseDto>> GetAllDepartmentsAsync()
    {
        var departments = await departmentRepository.GetAllDepartmentsAsync();
        return departments.Select(MapToResponseDto).ToList();
    }

    public async Task<List<DepartmentResponseDto>> GetActiveDepartmentsAsync()
    {
        var departments = await departmentRepository.GetActiveDepartmentsAsync();
        return departments.Select(MapToResponseDto).ToList();
    }

    public async Task<DepartmentResponseDto?> GetDepartmentByIdAsync(Guid id)
    {
        var department = await departmentRepository.GetByIdAsync(id);
        return department == null ? null : MapToResponseDto(department);
    }


    public async Task<DepartmentResponseDto> CreateDepartmentAsync(CreateDepartmentRequestDto dto)
    {
        var currentUserId = currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad för att skapa avdelningar.");


        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            IsActive = dto.IsActive,
            CreatedDate = DateTime.UtcNow,
            CreatedByUserId = currentUserId
        };

        await departmentRepository.AddAsync(department);
        await departmentRepository.SaveChangesAsync();

        logger.LogInformation("Department '{DepartmentName}' created by user {UserId}", dto.Name, currentUserId);

        // Return the created department with navigation properties loaded
        var created = await departmentRepository.GetByIdAsync(department.Id);
        return MapToResponseDto(created!);
    }

    public async Task<DepartmentResponseDto> UpdateDepartmentAsync(UpdateDepartmentRequestDto dto)
    {
        var currentUserId = currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad för att uppdatera avdelningar.");

        var department = await departmentRepository.GetByIdAsync(dto.Id);
        if (department == null)
            throw new InvalidOperationException("Avdelningen hittades inte.");


        department.Name = dto.Name;
        department.Description = dto.Description;
        department.IsActive = dto.IsActive;
        department.UpdatedDate = DateTime.UtcNow;
        department.UpdatedByUserId = currentUserId;

        departmentRepository.Update(department);
        await departmentRepository.SaveChangesAsync();

        logger.LogInformation("Department '{DepartmentName}' updated by user {UserId}", dto.Name, currentUserId);

        // Return the updated department with navigation properties loaded
        var updated = await departmentRepository.GetByIdAsync(department.Id);
        return MapToResponseDto(updated!);
    }

    public async Task<bool> DeleteDepartmentAsync(Guid id)
    {
        var currentUserId = currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad för att ta bort avdelningar.");

        var department = await departmentRepository.GetByIdAsync(id);
        if (department == null)
            return false;

        departmentRepository.Delete(department);
        await departmentRepository.SaveChangesAsync();

        logger.LogInformation("Department '{DepartmentName}' deleted by user {UserId}", department.Name, currentUserId);

        return true;
    }

    private static DepartmentResponseDto MapToResponseDto(Department department)
    {
        return new DepartmentResponseDto
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description,
            IsActive = department.IsActive,
            CreatedDate = department.CreatedDate,
            UpdatedDate = department.UpdatedDate,
            CreatedByUserName = department.CreatedByUser?.Email ?? "Okänd",
            UpdatedByUserName = department.UpdatedByUser?.Email
        };
    }
}