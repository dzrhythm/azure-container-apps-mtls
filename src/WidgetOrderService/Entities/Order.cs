namespace WidgetOrderService.Entities
{
    public class Order
    {
        public long Id { get; set; }

        public int Count { get; set; }

        public DateTime Created { get; } = DateTime.Now;
    }
}
