using Kaits.Application.DTOs; 
using MediatR; 
namespace Kaits.Application.Commands.CreateOrder; 
public record CreateOrderCommand(int CustomerId, System.Collections.Generic.List<OrderItemDto> Items):IRequest<int>;