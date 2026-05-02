namespace AlertaTempranaAPI.Layers.Dtos.Gmail
{
    public class GmailSettings
    {
        public string SenderEmail { get; init; } = string.Empty;
        public string AppPassword { get; init; } = string.Empty;
        public string RecipientEmail { get; init; } = string.Empty;
    }
}
