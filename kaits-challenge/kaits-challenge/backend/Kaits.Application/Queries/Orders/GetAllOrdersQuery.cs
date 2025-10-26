using Kaits.Application.DTOs; 
using MediatR; 
namespace Kaits.Application.Queries.Orders; 
public record GetAllOrdersQuery():IRequest<System.Collections.Generic.List<OrderReadDto>>;