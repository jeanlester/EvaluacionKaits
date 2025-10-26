using FluentValidation;
using FluentValidation.AspNetCore;
using Kaits.Application.Behaviors;
using Kaits.Application.Commands.CreateOrder;
using Kaits.Application.Interfaces;
using Kaits.Infrastructure.Handlers;
using Kaits.Infrastructure.Persistence;
using Kaits.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration).WriteTo.Console());

builder.Services.AddDbContext<KaitsDbContext>(opt =>
{
    var cs = builder.Configuration.GetConnectionString("Default") ?? "Server=(localdb)\\MSSQLLocalDB;Database=Kaits;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
    opt.UseSqlServer(cs);
});

builder.Services.AddScoped<IDateTime, DateTimeProvider>();
builder.Services.AddMediatR(typeof(CreateOrderCommand).Assembly, typeof(CreateOrderHandler).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();

builder.Services.AddCors(opt => opt.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<Kaits.WebApi.Middlewares.ErrorHandlingMiddleware>();
app.UseSwagger(); app.UseSwaggerUI();
app.UseCors();
app.MapControllers();
app.Run();
