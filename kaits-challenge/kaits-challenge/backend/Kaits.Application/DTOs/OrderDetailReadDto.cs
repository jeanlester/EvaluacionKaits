namespace Kaits.Application.DTOs; 
public record OrderDetailReadDto(int ProductId,string ProductDescription,int Quantity,decimal UnitPrice,decimal Subtotal);