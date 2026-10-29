using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication5.Auth;
using WebApplication5.DTOs;
using WebApplication5.Services;

namespace WebApplication5.Controllers;

[Authorize]
[Route("api/reservations")]
public class ReservationsController(IReservationService service, IAuthorizationService authorization) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var pagingError = ValidatePaging(page, pageSize);
        if (pagingError is not null)
        {
            return pagingError;
        }

        var result = User.IsAdmin()
            ? await service.GetPagedAsync(page, pageSize)
            : await service.GetPagedForUserAsync(User.GetUserId(), page, pageSize);

        SetTotalCount(result.TotalCount);
        return Ok(result.Items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReservationDTO>> GetById(int id)
    {
        var reservation = await service.GetByIdAsync(id);
        if (reservation is null)
        {
            return NotFound();
        }

        if (!await IsOwnerOrAdminAsync(reservation))
        {
            return Forbid();
        }

        return Ok(reservation);
    }

    [HttpPost]
    public async Task<IActionResult> Reserve(CreateReservationDTO dto)
    {
        var result = await service.ReserveAsync(User.GetUserId(), dto);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.ReservationId }, result.Value)
            : ErrorResult(result);
    }

    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var reservation = await service.GetByIdAsync(id);
        if (reservation is null)
        {
            return NotFound(new { error = $"Резерв с id={id} не найден." });
        }

        if (!await IsOwnerOrAdminAsync(reservation))
        {
            return Forbid();
        }

        var result = await service.CancelAsync(id);
        return result.IsSuccess ? Ok(result.Value) : ErrorResult(result);
    }

    private async Task<bool> IsOwnerOrAdminAsync(ReservationDTO reservation) =>
        (await authorization.AuthorizeAsync(User, reservation, Policies.OwnerOrAdmin)).Succeeded;
}
