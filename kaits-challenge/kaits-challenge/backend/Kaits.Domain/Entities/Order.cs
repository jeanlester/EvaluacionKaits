namespace Kaits.Domain.Entities; 
public class Order { 
    public int Id{get;set;} 
    public System.DateTime OrderDate{get;set;} 
    public int CustomerId{get;set;} 
    public Customer? Customer{get;set;} 
    public decimal Total{get;set;} 
    public System.Collections.Generic.ICollection<OrderDetail> Details{get;set;}=new System.Collections.Generic.List<OrderDetail>(); 
}