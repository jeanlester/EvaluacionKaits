namespace Kaits.Domain.Entities; 
public class OrderDetail { 
    public int Id{get;set;} 
    public int OrderId{get;set;} 
    public Order? Order{get;set;} 
    public int ProductId{get;set;} 
    public Product? Product{get;set;} 
    public string ProductDescription{get;set;}=string.Empty; 
    public int Quantity{get;set;} 
    public decimal UnitPrice{get;set;} 
    public decimal Subtotal{get;set;} 
}