namespace GylleneDroppen.Application.Interfaces.Admin;

public interface IAdminService
{
    Task<ServiceResponse<MessageResponse>> PromoteUserToAdminAsync(Guid userId);
    Task<ServiceResponse<MessageResponse>> DemoteUserToAdminAsync(DemoteAdminRequest request);
    Task<ServiceResponse<List<AdminResponse>>> GetAllAdminsAsync();
}