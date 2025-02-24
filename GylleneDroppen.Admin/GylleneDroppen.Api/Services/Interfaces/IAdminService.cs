using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Dtos.Admin;
using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services.Interfaces;

public interface IAdminService
{
    Task<ServiceResponse<MessageResponse>> PromoteUserToAdminAsync(Guid userId);
    Task<ServiceResponse<MessageResponse>> DemoteUserToAdminAsync(DemoteAdminRequest request);
}