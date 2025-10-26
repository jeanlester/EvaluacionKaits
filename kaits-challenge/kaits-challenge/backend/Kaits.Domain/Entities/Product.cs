namespace Kaits.Domain.Entities;
public class Product { 
    public int Id{get;set;} 
    public string Description{get;set;}=string.Empty; 
    public decimal UnitPrice{get;set;} 
    public System.Collections.Generic.ICollection<OrderDetail> OrderDetails{get;set;}=new System.Collections.Generic.List<OrderDetail>(); 
}