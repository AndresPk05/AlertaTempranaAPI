namespace AlertaTempranaAPI.Layers.Infrastructure;

public interface IEmailQueueService
{
    ValueTask EnqueueAsync(EmailTask task, CancellationToken ct = default);
    ValueTask<EmailTask> DequeueAsync(CancellationToken ct = default);
    int Count { get; }
}

public record EmailTask(string Subject, string HtmlBody, DateTime EnqueuedAt);