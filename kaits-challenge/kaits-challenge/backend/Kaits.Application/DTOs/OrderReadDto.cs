namespace Kaits.Application.DTOs; 
public record OrderReadDto(int Id, System.DateTime OrderDate, string Customer, decimal Total, System.Collections.Generic.List<OrderDetailReadDto> Details);