using LastMile.Api.Dtos;
using LastMile.Application.Entregas;
using LastMile.Domain.Entregas;
using Microsoft.AspNetCore.Mvc;

namespace LastMile.Api.Controllers;

[ApiController]
[Route("deliveries")]
public sealed class DeliveriesController : ControllerBase
{
    private readonly AssignCourierUseCase _useCase;

    public DeliveriesController(AssignCourierUseCase useCase)
        => _useCase = useCase;

    [HttpPost("{id:guid}/assign-courier")]
    [ProducesResponseType(typeof(AssignCourierResult), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> AssignCourier(
        Guid id,
        [FromBody] AssignCourierRequest body,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(body.RequestedBy))
            return BadRequest(new { error = "RequestedBy is required" });

        try
        {
            var result = await _useCase.ExecuteAsync(
                new AssignCourierCommand(id, body.CourierId, body.RequestedBy),
                ct);
            return Ok(result);
        }
        catch (DomainException ex) when (ex.Message == "Delivery not found")
        {
            return NotFound(new { error = ex.Message });
        }
        catch (DomainException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
}