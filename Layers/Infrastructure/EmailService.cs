using AlertaTempranaAPI.Layers.Dtos.Gmail;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace AlertaTempranaAPI.Layers.Infrastructure
{
    public class EmailService : IEmailService
    {
        private readonly GmailSettings _settings;
        private readonly SmtpClient _smtp;

        public EmailService(IConfiguration configuration)
        {
            _settings = configuration.GetSection(nameof(GmailSettings)).Get<GmailSettings>()
                ?? throw new InvalidOperationException("La sección 'GmailSettings' no está configurada en appsettings.json.");

            _smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_settings.SenderEmail, _settings.AppPassword)
            };
        }

        public async Task SendAsync(string subject, string htmlBody, CancellationToken ct = default)
        {
            using var mail = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8
            };

            mail.To.Add(_settings.RecipientEmail);

            await _smtp.SendMailAsync(mail, ct);
        }

        public void Dispose() => _smtp.Dispose();
    }
}
