using Kaits.Application.Commands.CreateOrder;
using Kaits.Application.Queries.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kaits.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    public OrdersController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand cmd, CancellationToken ct)
    {
        var id = await _mediator.Send(cmd, ct);
        return Ok(new { orderId = id });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllOrdersQuery(), ct);
        return Ok(result);
    }
}
