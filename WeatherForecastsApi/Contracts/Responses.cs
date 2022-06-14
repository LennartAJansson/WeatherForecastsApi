namespace WeatherForecastsApi.Contracts
{
    public record WeatherResponse(int Id, DateTime Date, double TemperatureC, double TemperatureF, string? Summary);
}
