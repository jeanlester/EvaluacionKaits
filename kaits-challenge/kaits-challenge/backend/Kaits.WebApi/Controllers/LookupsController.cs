using Kaits.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kaits.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LookupsController : ControllerBase
{
    private readonly KaitsDbContext _db;
    public LookupsController(KaitsDbContext db) => _db = db;

    [HttpGet("customers")]
    public async Task<IActionResult> GetCustomers()
    {
        var data = await _db.Customers.OrderBy(c => c.FullName).Select(c => new { id = c.Id, fullName = c.FullName, dni = c.Dni }).ToListAsync();
        return Ok(data);
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetProducts()
    {
        var data = await _db.Products.OrderBy(p => p.Description).Select(p => new { id = p.Id, description = p.Description, unitPrice = p.UnitPrice }).ToListAsync();
        return Ok(data);
    }
}
