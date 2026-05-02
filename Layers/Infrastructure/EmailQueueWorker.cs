namespace AlertaTempranaAPI.Layers.Infrastructure;

public class EmailQueueWorker : BackgroundService
{
    private readonly IEmailQueueService _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<EmailQueueWorker> _logger;

    public EmailQueueWorker(IEmailQueueService queue, IServiceScopeFactory scopeFactory, ILogger<EmailQueueWorker> logger)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("EmailQueueWorker started at {time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var task = await _queue.DequeueAsync(stoppingToken);
                await ProcessEmailAsync(task, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing email from queue");
            }
        }

        _logger.LogInformation("EmailQueueWorker stopped at {time}", DateTimeOffset.Now);
    }

    private async Task ProcessEmailAsync(EmailTask task, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

        try
        {
            await emailService.SendAsync(task.Subject, task.HtmlBody, ct);
            _logger.LogInformation("Email sent: {Subject}", task.Subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email: {Subject}", task.Subject);
        }
    }
}