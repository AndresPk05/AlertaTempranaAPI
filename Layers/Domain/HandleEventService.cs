using AlertaTempranaAPI.Layers.Domain.StrategiesEvent;
using AlertaTempranaAPI.Layers.Dtos;
using AlertaTempranaAPI.Layers.Dtos.Alerts;

namespace AlertaTempranaAPI.Layers.Domain
{
    public class HandleEventService
    {
        private readonly Dictionary<EventType, IStrategyEvent> strategies;
        private readonly ILogger<PositionEventStrategy> _logger;
        public HandleEventService(IServiceProvider serviceProvider, ILogger<PositionEventStrategy> logger)
        {
            strategies = new Dictionary<EventType, IStrategyEvent>
            {
                { EventType.Position, serviceProvider.GetRequiredService<PositionEventStrategy>() },
                { EventType.Emergency, serviceProvider.GetRequiredService<EmergencyEventStrategy>() },
            };
            _logger = logger;
        }

        public async Task<Result<bool>> handle(RequestAlert request)
        {
            try
            {
                _logger.LogInformation("Reception event type {Time} to vehicule {plate}", DateTime.Now, request.VehiculePlate);
                strategies.TryGetValue(request.Type, out var strategy);
                if (strategy is null) return Result<bool>.Error("event type not configured");

                return await strategy.handleAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en el evento");
                throw;
            }

        }
    }
}
