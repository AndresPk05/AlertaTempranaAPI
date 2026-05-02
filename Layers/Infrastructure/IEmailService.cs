namespace AlertaTempranaAPI.Layers.Infrastructure
{
    public interface IEmailService
    {
        void Dispose();
        Task SendAsync(string subject, string htmlBody, CancellationToken ct = default);
    }
}