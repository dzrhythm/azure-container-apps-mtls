using System.Text.Json;
using Microsoft.Extensions.Options;

namespace WidgetOrderApp.OrderService;

public class OrderServiceAgent
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    };

    private readonly IOptionsMonitor<OrderServiceOptions> _options;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;

    public OrderServiceAgent(IOptionsMonitor<OrderServiceOptions> options, IHttpClientFactory httpClientFactory, ILogger<OrderServiceAgent> logger)
    {
        _options = options;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<Order?> PlaceOrderAsync(OrderRequest request)
    {
        _logger.LogInformation("Ordering {Count} widgets", request.Count);

        try
        {
            OrderServiceOptions options = _options.CurrentValue;
            HttpClient client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(options.OrderUri ?? throw new InvalidOperationException($"{nameof(options.OrderUri)} is not set in configuration"));

            HttpResponseMessage response = await client.PostAsJsonAsync("Orders", request);
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            Order? order = JsonSerializer.Deserialize<Order?>(responseContent, JsonSerializerOptions);

            _logger.LogInformation("Successfully ordered {Count} widgets", request.Count);

            return order;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error ordering {Count} widgets", request.Count);
            throw;
        }
    }
}
