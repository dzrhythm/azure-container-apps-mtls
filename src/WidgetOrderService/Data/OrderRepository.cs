using System.Collections.ObjectModel;
using WidgetOrderService.Entities;

namespace WidgetOrderService.Data
{
    public class OrderRepository
    {
        private readonly object _locker = new();
        private long _nextId = 1;
        private readonly Dictionary<long, Order> _orders = new();

        public Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default)
        {
            lock(_locker)
            {
                order.Id = _nextId++;
                _orders[order.Id] = (order);
            }

            return Task.FromResult(order);
        }

        public Task<Order?> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_orders.ContainsKey(id) ? _orders[id] : default);
        }
    }
}
