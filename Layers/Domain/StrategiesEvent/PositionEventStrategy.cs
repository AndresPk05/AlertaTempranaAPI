using AlertaTempranaAPI.Layers.Dtos;
using AlertaTempranaAPI.Layers.Dtos.Alerts;

namespace AlertaTempranaAPI.Layers.Domain.StrategiesEvent
{
    public class PositionEventStrategy : IStrategyEvent
    {
        private readonly ILogger<PositionEventStrategy> _logger;

        public PositionEventStrategy(ILogger<PositionEventStrategy> logger)
        {
            _logger = logger;
        }
        public async Task<Result<bool>> handleAsync(RequestAlert requestAlert)
        {
            _logger.LogInformation("Reception event type position {Time} to vehicule {plate}", DateTime.Now, requestAlert.VehiculePlate);
            return Result<bool>.Ok(true);
        }
    }
}
