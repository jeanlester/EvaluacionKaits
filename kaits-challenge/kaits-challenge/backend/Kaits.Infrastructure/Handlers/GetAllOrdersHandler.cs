using Kaits.Application.DTOs;
using Kaits.Application.Queries.Orders;
using Kaits.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kaits.Infrastructure.Handlers;

public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, List<OrderReadDto>>
{
    private readonly KaitsDbContext _db;
    public GetAllOrdersHandler(KaitsDbContext db) => _db = db;

    public async Task<List<OrderReadDto>> Handle(GetAllOrdersQuery request, CancellationToken ct)
    {
        var orders = await _db.Orders.AsNoTracking()
            .Include(o => o.Customer)
            .Include(o => o.Details)
            .OrderByDescending(o => o.Id)
            .ToListAsync(ct);

        return orders.Select(o => new OrderReadDto(
            o.Id,
            o.OrderDate,
            o.Customer!.FullName,
            o.Total,
            o.Details.Select(d => new OrderDetailReadDto(d.ProductId, d.ProductDescription, d.Quantity, d.UnitPrice, d.Subtotal)).ToList()
        )).ToList();
    }
}
