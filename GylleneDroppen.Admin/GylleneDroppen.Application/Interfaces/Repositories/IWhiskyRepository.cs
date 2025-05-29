using GylleneDroppen.Application.Dtos.Whisky;

namespace GylleneDroppen.Application.Interfaces.Repositories;

public interface IWhiskyRepository : IRepository<Whisky>
{
    Task<WhiskySearchResultDto> SearchWhiskiesAsync(WhiskySearchDto searchDto);
    Task<Whisky?> GetWhiskyWithDetailsAsync(Guid id);
    Task<List<Whisky>> GetWhiskiesByIdsAsync(List<Guid> ids);
    Task<bool> ExistsByNameAndDistilleryAsync(string name, string distillery, Guid? excludeId = null);
}