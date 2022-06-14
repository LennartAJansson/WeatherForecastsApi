namespace WeatherForecastsApi.Contracts
{
    using MediatR;

    public record CreateWeatherForecast(DateTime Date, double TemperatureC, string? Summary) : IRequest<WeatherResponse>;
    public record UpdateWeatherForecast(int Id, DateTime Date, double TemperatureC, string? Summary) : IRequest<WeatherResponse>;
    public record DeleteWeatherForecast(int Id) : IRequest<WeatherResponse>;

}
