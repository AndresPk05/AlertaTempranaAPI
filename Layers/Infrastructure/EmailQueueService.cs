using System.Threading.Channels;

namespace AlertaTempranaAPI.Layers.Infrastructure;

public class EmailQueueService : IEmailQueueService
{
    private readonly Channel<EmailTask> _queue;
    private int _count;

    public EmailQueueService()
    {
        _queue = Channel.CreateBounded<EmailTask>(new BoundedChannelOptions(10000)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = false
        });
    }

    public async ValueTask EnqueueAsync(EmailTask task, CancellationToken ct = default)
    {
        await _queue.Writer.WriteAsync(task, ct);
        Interlocked.Increment(ref _count);
    }

    public async ValueTask<EmailTask> DequeueAsync(CancellationToken ct = default)
    {
        var task = await _queue.Reader.ReadAsync(ct);
        Interlocked.Decrement(ref _count);
        return task;
    }

    public int Count => _count;
}