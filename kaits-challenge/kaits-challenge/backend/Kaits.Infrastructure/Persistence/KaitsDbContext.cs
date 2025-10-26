using Kaits.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kaits.Infrastructure.Persistence;

public class KaitsDbContext : DbContext
{
    public KaitsDbContext(DbContextOptions<KaitsDbContext> options) : base(options) {}
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(b => { b.ToTable("Clientes"); b.HasKey(x=>x.Id); b.Property(x=>x.FullName).IsRequired().HasMaxLength(160); b.Property(x=>x.Dni).IsRequired().HasMaxLength(15); });
        modelBuilder.Entity<Product>().ToTable("Productos");
        modelBuilder.Entity<Order>(b => { b.ToTable("Pedidos"); b.HasKey(x=>x.Id); b.HasOne(x=>x.Customer).WithMany(c=>c.Orders).HasForeignKey(x=>x.CustomerId); b.Property(x=>x.Total).HasPrecision(18,2); });
        modelBuilder.Entity<OrderDetail>(b => { b.ToTable("PedidoDetalles"); b.HasKey(x=>x.Id); b.HasOne(x=>x.Order).WithMany(o=>o.Details).HasForeignKey(x=>x.OrderId); b.HasOne(x=>x.Product).WithMany(p=>p.OrderDetails).HasForeignKey(x=>x.ProductId); b.Property(x=>x.UnitPrice).HasPrecision(18,2); b.Property(x=>x.Subtotal).HasPrecision(18,2); });
    }
}
