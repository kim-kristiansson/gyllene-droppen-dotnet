using GylleneDroppen.Admin.Api.Dtos.Admin;
using GylleneDroppen.Shared.Dtos.Generic;
using GylleneDroppen.Shared.Utils;

namespace GylleneDroppen.Admin.Api.Interfaces.Services;

public interface IAdminService
{
    Task<ServiceResponse<MessageResponse>> PromoteUserToAdminAsync(Guid userId);
    Task<ServiceResponse<MessageResponse>> DemoteUserToAdminAsync(DemoteAdminRequest request);
    Task<ServiceResponse<List<AdminResponse>>> GetAllAdminsAsync();
}