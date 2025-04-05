using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Admin;
using GylleneDroppen.Application.Dtos.Admin.Admin;
using GylleneDroppen.Application.Dtos.Common;

namespace GylleneDroppen.Application.Interfaces.Services.Admin;

public interface IAdminService
{
    Task<Result<MessageResponse>> PromoteUserToAdminAsync(Guid userId);
    Task<Result<MessageResponse>> DemoteUserToAdminAsync(DemoteAdminRequest request);
    Task<Result<List<AdminResponse>>> GetAllAdminsAsync();
}