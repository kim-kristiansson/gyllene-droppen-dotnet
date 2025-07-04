using GylleneDroppen.Application.Dtos.Department;

namespace GylleneDroppen.Application.Interfaces.Services;

public interface IDepartmentService
{
    Task<List<DepartmentResponseDto>> GetAllDepartmentsAsync();
    Task<List<DepartmentResponseDto>> GetActiveDepartmentsAsync();
    Task<DepartmentResponseDto?> GetDepartmentByIdAsync(Guid id);
    Task<DepartmentResponseDto> CreateDepartmentAsync(CreateDepartmentRequestDto dto);
    Task<DepartmentResponseDto> UpdateDepartmentAsync(UpdateDepartmentRequestDto dto);
    Task<bool> DeleteDepartmentAsync(Guid id);
}