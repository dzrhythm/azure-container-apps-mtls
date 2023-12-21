using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WidgetOrderApp.Data;
using WidgetOrderApp.OrderService;

namespace WidgetOrderApp.Pages;

public class OrderWidgetsModel : PageModel
{
    private readonly OrderServiceAgent _orderServiceAgent;
    private readonly OrderContext _orderContext;
    private readonly ILogger _logger;

    public OrderWidgetsModel(OrderServiceAgent orderServiceAgent, OrderContext orderContext, IConfiguration config, ILogger<OrderWidgetsModel> logger)
    {
        _orderServiceAgent = orderServiceAgent;
        _orderContext = orderContext;
        _logger = logger;
    }

    [BindProperty]
    public OrderRequest OrderRequest { get; set; } = new OrderRequest { Count = 1 };

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (OrderRequest != null)
        {
            Order? newOrder = await _orderServiceAgent.PlaceOrderAsync(OrderRequest);
            if (newOrder != null)
            {
                _orderContext.Add(newOrder);
                _orderContext.SaveChanges();
            }
        }

        return RedirectToPage("./Index");
    }
}


