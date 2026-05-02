using AlertaTempranaAPI.Layers.Domain.Constants;
using AlertaTempranaAPI.Layers.Dtos;
using AlertaTempranaAPI.Layers.Dtos.Alerts;
using AlertaTempranaAPI.Layers.Infrastructure;

namespace AlertaTempranaAPI.Layers.Domain.StrategiesEvent
{
    public class EmergencyEventStrategy : IStrategyEvent
    {
        private readonly ILogger<EmergencyEventStrategy> _logger;
        private readonly IEmailQueueService _emailQueue;

        public EmergencyEventStrategy(ILogger<EmergencyEventStrategy> logger, IEmailQueueService emailQueue)
        {
            _logger = logger;
            _emailQueue = emailQueue;
        }

        public async Task<Result<bool>> handleAsync(RequestAlert requestAlert)
        {
            try
            {
                _logger.LogInformation("Reception event type Emergency at {Time} to vehicule {plate}", DateTime.Now, requestAlert.VehiculePlate);

                var emailTask = new EmailTask(
                    $"Emergency vehicule {requestAlert.VehiculePlate}",
                    EmailTemplate.EmergencyAlert(requestAlert),
                    DateTime.Now
                );

                await _emailQueue.EnqueueAsync(emailTask);

                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enqueuing email for vehicule {plate}", requestAlert.VehiculePlate);
                return Result<bool>.Error(ex.Message);
            }
        }
    }
}
