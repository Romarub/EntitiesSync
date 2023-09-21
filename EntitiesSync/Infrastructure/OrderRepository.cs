using EntitiesSync.Model;

namespace EntitiesSync.Infrastructure;

public class OrderRepository : IOrderRepository
{
    public IQueryable<Order> GetOrders()
    {
        return new[] {
            new Order { Id = 1, DateTimeUtc = DateTime.UtcNow, Status = OrderStatus.Pending, Customer = new Customer { Id = 1 } },
            new Order { Id = 2, DateTimeUtc = DateTime.UtcNow, Status = OrderStatus.Pending, Customer = new Customer { Id = 1 } },
            new Order { Id = 3, DateTimeUtc = DateTime.UtcNow, Status = OrderStatus.Pending, Customer = new Customer { Id = 1 } },
            new Order { Id = 4, DateTimeUtc = DateTime.UtcNow, Status = OrderStatus.Pending, Customer = new Customer { Id = 1 } },
            new Order { Id = 5, DateTimeUtc = DateTime.UtcNow, Status = OrderStatus.Pending, Customer = new Customer { Id = 1 } },
        }
        .AsQueryable();
    }
}
