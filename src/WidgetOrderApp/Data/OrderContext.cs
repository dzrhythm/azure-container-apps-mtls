using Microsoft.EntityFrameworkCore;
using WidgetOrderApp.OrderService;

namespace WidgetOrderApp.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders => Set<Order>();
    }
}
