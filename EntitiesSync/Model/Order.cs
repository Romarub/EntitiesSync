namespace EntitiesSync.Model;

public class Order
{
    // private Order() { }
    public int Id { get ; set;}
    public DateTime DateTimeUtc { get; set; }
    public Customer Customer { get; set; }
    public OrderStatus Status { get; set; }
}

public class Customer
{

    public int Id { get ; set;}

}

public enum OrderStatus
{
    Pending,
    Cancelled,
    Shipped,
    Completed
}