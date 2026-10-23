using Microsoft.AspNetCore.Mvc;
using WebApplication5.DTOs;
using WebApplication5.Services;

namespace WebApplication5.Controllers;

public abstract class CrudController<TRead, TCreate, TUpdate, TPatch> : ApiControllerBase
    where TRead : class, IIdentifiable
{
    private readonly ICrudService<TRead, TCreate, TUpdate, TPatch> _service;

    protected CrudController(ICrudService<TRead, TCreate, TUpdate, TPatch> service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TRead>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var pagingError = ValidatePaging(page, pageSize);
        if (pagingError is not null)
        {
            return pagingError;
        }

        var result = await _service.GetPagedAsync(page, pageSize);
        SetTotalCount(result.TotalCount);
        return Ok(result.Items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TRead>> GetById(int id)
    {
        var dto = await _service.GetByIdAsync(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TCreate dto)
    {
        var result = await _service.CreateAsync(dto);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
            : ErrorResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TUpdate dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result.IsSuccess ? NoContent() : ErrorResult(result);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, TPatch dto)
    {
        var result = await _service.PatchAsync(id, dto);
        return result.IsSuccess ? NoContent() : ErrorResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result.IsSuccess ? NoContent() : ErrorResult(result);
    }
}
