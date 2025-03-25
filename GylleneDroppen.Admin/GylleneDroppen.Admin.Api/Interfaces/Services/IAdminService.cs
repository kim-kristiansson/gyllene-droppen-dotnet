using GylleneDroppen.Admin.Api.Dtos.Admin;
using GylleneDroppen.Core.Common;
using GylleneDroppen.Core.Dtos.Generic;

namespace GylleneDroppen.Admin.Api.Interfaces.Services;

public interface IAdminService
{
    Task<Result<MessageResponse>> PromoteUserToAdminAsync(Guid userId);
    Task<Result<MessageResponse>> DemoteUserToAdminAsync(DemoteAdminRequest request);
    Task<Result<List<AdminResponse>>> GetAllAdminsAsync();
}