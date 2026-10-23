using Microsoft.AspNetCore.Mvc;
using WebApplication5.Services;

namespace WebApplication5.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult ErrorResult(ServiceResult result) => result.Status switch
    {
        ServiceStatus.NotFound => NotFound(new { error = result.Error }),
        ServiceStatus.Conflict => Conflict(new { error = result.Error }),
        _ => BadRequest(new { error = result.Error })
    };

    protected ActionResult? ValidatePaging(int page, int pageSize) =>
        page < 1 || pageSize < 1 || pageSize > 100
            ? BadRequest(new { error = "page должен быть >= 1, pageSize — от 1 до 100." })
            : null;

    protected void SetTotalCount(int totalCount) =>
        Response.Headers["X-Total-Count"] = totalCount.ToString();
}
