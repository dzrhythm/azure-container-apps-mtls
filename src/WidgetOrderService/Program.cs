using Microsoft.AspNetCore.HttpLogging;
using WidgetOrderService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.RequestHeaders.Add("sec-ch-ua-mobile");
    logging.RequestHeaders.Add("sec-ch-ua-platform");
    logging.RequestHeaders.Add("Sec-Fetch-Site");
    logging.RequestHeaders.Add("Sec-Fetch-Mode");
    logging.RequestHeaders.Add("Sec-Fetch-Dest");
    logging.RequestHeaders.Add("x-forwarded-for");
    logging.RequestHeaders.Add("x-envoy-external-address");
    logging.RequestHeaders.Add("x-forwarded-proto");
    logging.RequestHeaders.Add("x-arr-ssl");
    logging.RequestHeaders.Add("Origin");
    logging.RequestHeaders.Add("Referer");
    logging.RequestHeaders.Add("x-request-id");
    logging.RequestHeaders.Add("traceparent");
    logging.RequestHeaders.Add("x-k8se-app-name");
    logging.RequestHeaders.Add("x-k8se-app-namespace");
    logging.RequestHeaders.Add("x-k8se-protocol");
    logging.RequestHeaders.Add("x-k8se-app-kind");
    logging.RequestHeaders.Add("x-ms-containerapp-name");
    logging.RequestHeaders.Add("x-ms-containerapp-revision-name");
    logging.RequestHeaders.Add("x-k8se-app-kind");
    logging.RequestHeaders.Add("x-k8se-app-kind");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

builder.Services.AddSingleton<OrderRepository>();

var app = builder.Build();

app.UseHttpLogging();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
