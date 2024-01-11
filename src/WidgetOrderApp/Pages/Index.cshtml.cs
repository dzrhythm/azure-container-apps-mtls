using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WidgetOrderApp.Data;
using WidgetOrderApp.OrderService;

namespace WidgetOrderApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly OrderContext _orderContext;


    public IndexModel(OrderContext orderContext, ILogger<IndexModel> logger)
    {
        _orderContext = orderContext;
        _logger = logger;
    }

    public IList<Order>? Orders { get; set; }

    public async Task OnGetAsync()
    {
        Orders = await _orderContext.Orders.ToListAsync();
    }
}
