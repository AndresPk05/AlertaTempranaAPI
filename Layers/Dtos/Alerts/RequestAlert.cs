using System.Text.Json.Serialization;

namespace AlertaTempranaAPI.Layers.Dtos.Alerts
{
    public record RequestAlert
    {
        [JsonPropertyName("type")]
        public EventType Type { get; init; }

        [JsonPropertyName("vehicle_plate")]
        public required string VehiculePlate { get; init; }

        [JsonPropertyName("coordinates")]
        public required Coordinate Coordinates { get; init; }

        [JsonPropertyName("status")]
        public StatusVehicule Status { get; init; }
    }
}
