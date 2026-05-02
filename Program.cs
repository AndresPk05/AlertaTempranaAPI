using AlertaTempranaAPI.Layers.Infrastructure;
using AlertaTempranaAPI.Layers.Domain.StrategiesEvent;
using AlertaTempranaAPI.Layers.Domain;
using AlertaTempranaAPI.Layers.Dtos.Alerts;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxConcurrentConnections = 1000;
    options.Limits.MaxConcurrentUpgradedConnections = 1000;
    options.Limits.MaxRequestBodySize = 1024 * 1024;
    options.Limits.MinRequestBodyDataRate = null;
    options.Limits.MinResponseDataRate = null;
    options.ListenAnyIP(8080);
});

builder.Services.AddOpenApi();
builder.Services.AddSingleton<IEmailQueueService, EmailQueueService>();
builder.Services.AddHostedService<EmailQueueWorker>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<PositionEventStrategy>();
builder.Services.AddScoped<EmergencyEventStrategy>();
builder.Services.AddScoped<HandleEventService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapPost("event", async (RequestAlert request, HandleEventService handleEventService) =>
{
    var result = await handleEventService.handle(request);
    return result.Successful ? Results.Ok(result.Value) : Results.InternalServerError(result.Message);
});

app.MapGet("heath", () => Results.Ok());

app.MapGet("queue-status", (IEmailQueueService queue) => Results.Ok(new { QueueLength = queue.Count }));

app.Run();