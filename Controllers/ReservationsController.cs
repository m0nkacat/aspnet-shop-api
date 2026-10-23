using Microsoft.AspNetCore.Mvc;
using WebApplication5.DTOs;
using WebApplication5.Services;

namespace WebApplication5.Controllers;

[Route("api/reservations")]
public class ReservationsController : ApiControllerBase
{
    private readonly IReservationService _service;

    public ReservationsController(IReservationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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
    public async Task<ActionResult<ReservationDTO>> GetById(int id)
    {
        var dto = await _service.GetByIdAsync(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Reserve(CreateReservationDTO dto)
    {
        var result = await _service.ReserveAsync(dto);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.ReservationId }, result.Value)
            : ErrorResult(result);
    }

    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await _service.CancelAsync(id);
        return result.IsSuccess ? Ok(result.Value) : ErrorResult(result);
    }
}
