using AlertaTempranaAPI.Layers.Dtos;
using AlertaTempranaAPI.Layers.Dtos.Alerts;

namespace AlertaTempranaAPI.Layers.Domain.StrategiesEvent
{
    public interface IStrategyEvent
    {
        Task<Result<bool>> handleAsync(RequestAlert requestAlert);
    }
}
