using WebApplication5.DTOs;

namespace WebApplication5.Services;

public interface ICrudService<TRead, TCreate, TUpdate, TPatch>
    where TRead : class
{
    Task<PagedResult<TRead>> GetPagedAsync(int page, int pageSize);

    Task<TRead?> GetByIdAsync(int id);

    Task<ServiceResult<TRead>> CreateAsync(TCreate dto);

    Task<ServiceResult> UpdateAsync(int id, TUpdate dto);

    Task<ServiceResult> PatchAsync(int id, TPatch dto);

    Task<ServiceResult> DeleteAsync(int id);
}
