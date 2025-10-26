namespace Kaits.Domain.Entities; 
public class Customer { 
    public int Id{get;set;} 
    public string FullName{get;set;}=string.Empty; 
    public string Dni{get;set;}=string.Empty; 
    public System.Collections.Generic.ICollection<Order> Orders{get;set;}=new System.Collections.Generic.List<Order>(); 
}