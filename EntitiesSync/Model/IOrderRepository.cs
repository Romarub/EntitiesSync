namespace EntitiesSync.Model;

public interface IOrderRepository
{
    public IQueryable<Order> GetOrders();
}

