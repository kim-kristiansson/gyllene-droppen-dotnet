using GylleneDroppen.Admin.Application.Dtos.Admin;
using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Common;

namespace GylleneDroppen.Admin.Application.Interfaces.Services;

public interface IAdminService
{
    Task<Result<MessageResponse>> PromoteUserToAdminAsync(Guid userId);
    Task<Result<MessageResponse>> DemoteUserToAdminAsync(DemoteAdminRequest request);
    Task<Result<List<AdminResponse>>> GetAllAdminsAsync();
}