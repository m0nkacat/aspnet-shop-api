using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication5.DTOs;
using WebApplication5.Repositories;

namespace WebApplication5.Services;

public abstract class CrudService<TEntity, TRead, TCreate, TUpdate, TPatch>
    : ICrudService<TRead, TCreate, TUpdate, TPatch>
    where TEntity : class
    where TRead : class
{
    protected readonly IRepository<TEntity> Repository;
    protected readonly IMapper Mapper;

    protected CrudService(IRepository<TEntity> repository, IMapper mapper)
    {
        Repository = repository;
        Mapper = mapper;
    }

    protected abstract string EntityName { get; }

    protected virtual void OnCreating(TEntity entity, TCreate dto) { }

    protected virtual void OnUpdating(TEntity entity, TUpdate dto) { }

    protected virtual void OnPatching(TEntity entity, TPatch dto) { }

    public async Task<PagedResult<TRead>> GetPagedAsync(int page, int pageSize)
    {
        var (items, totalCount) = await Repository.GetPagedAsync(page, pageSize);
        return new PagedResult<TRead>(Mapper.Map<List<TRead>>(items), totalCount);
    }

    public async Task<TRead?> GetByIdAsync(int id)
    {
        var entity = await Repository.GetByIdAsync(id);
        return entity is null ? null : Mapper.Map<TRead>(entity);
    }

    public async Task<ServiceResult<TRead>> CreateAsync(TCreate dto)
    {
        var entity = Mapper.Map<TEntity>(dto);
        OnCreating(entity, dto);

        await Repository.AddAsync(entity);

        var error = await TrySaveAsync();
        if (error is not null)
        {
            return ServiceResult<TRead>.Fail(ServiceStatus.Invalid, error);
        }

        var created = await Repository.GetByIdAsync(Repository.GetId(entity));
        return ServiceResult<TRead>.Ok(Mapper.Map<TRead>(created));
    }

    public async Task<ServiceResult> UpdateAsync(int id, TUpdate dto)
    {
        var entity = await Repository.FindAsync(id);
        if (entity is null)
        {
            return NotFound(id);
        }

        Mapper.Map(dto, entity);
        OnUpdating(entity, dto);

        var error = await TrySaveAsync();
        return error is null ? ServiceResult.Ok() : ServiceResult.Fail(ServiceStatus.Invalid, error);
    }

    public async Task<ServiceResult> PatchAsync(int id, TPatch dto)
    {
        var entity = await Repository.FindAsync(id);
        if (entity is null)
        {
            return NotFound(id);
        }

        Mapper.Map(dto, entity);
        OnPatching(entity, dto);

        var error = await TrySaveAsync();
        return error is null ? ServiceResult.Ok() : ServiceResult.Fail(ServiceStatus.Invalid, error);
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var entity = await Repository.FindAsync(id);
        if (entity is null)
        {
            return NotFound(id);
        }

        Repository.Remove(entity);

        try
        {
            await Repository.SaveChangesAsync();
            return ServiceResult.Ok();
        }
        catch (DbUpdateException)
        {
            return ServiceResult.Fail(ServiceStatus.Conflict,
                $"{EntityName} с id={id} нельзя удалить: на запись ссылаются другие данные.");
        }
    }

    protected ServiceResult NotFound(int id) =>
        ServiceResult.Fail(ServiceStatus.NotFound, $"{EntityName}: запись с id={id} не найдена.");

    private async Task<string?> TrySaveAsync()
    {
        try
        {
            await Repository.SaveChangesAsync();
            return null;
        }
        catch (DbUpdateException ex)
        {
            return $"{EntityName}: не удалось сохранить, проверьте Id связанных сущностей. " +
                   (ex.InnerException?.Message ?? ex.Message);
        }
    }
}
