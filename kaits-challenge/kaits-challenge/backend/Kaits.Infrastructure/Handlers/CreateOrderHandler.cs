using Kaits.Application.Commands.CreateOrder;
using Kaits.Application.Interfaces;
using Kaits.Domain.Entities;
using Kaits.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kaits.Infrastructure.Handlers;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly KaitsDbContext _db;
    private readonly IDateTime _clock;
    public CreateOrderHandler(KaitsDbContext db, IDateTime clock) { _db = db; _clock = clock; }

    public async Task<int> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        var customer = await _db.Customers.FindAsync(new object?[] { request.CustomerId }, ct)
            ?? throw new InvalidOperationException("Cliente no existe.");

        var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();
        var products = await _db.Products.Where(p => productIds.Contains(p.Id)).ToDictionaryAsync(p => p.Id, ct);
        if (products.Count != productIds.Count) throw new InvalidOperationException("Uno o más productos no existen.");

        using var trx = await _db.Database.BeginTransactionAsync(ct);

        var order = new Order { CustomerId = customer.Id, OrderDate = _clock.UtcNow, Total = 0 };
        _db.Orders.Add(order);
        await _db.SaveChangesAsync(ct);

        decimal total = 0;
        foreach (var item in request.Items)
        {
            var prod = products[item.ProductId];
            var unitPrice = item.UnitPrice > 0 ? item.UnitPrice : prod.UnitPrice;
            var detail = new OrderDetail { OrderId = order.Id, ProductId = prod.Id, ProductDescription = prod.Description, Quantity = item.Quantity, UnitPrice = unitPrice, Subtotal = unitPrice * item.Quantity };
            total += detail.Subtotal;
            _db.OrderDetails.Add(detail);
        }

        order.Total = total;
        await _db.SaveChangesAsync(ct);
        await trx.CommitAsync(ct);
        return order.Id;
    }
}
