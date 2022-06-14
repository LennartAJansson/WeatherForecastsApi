namespace WeatherForecastsApi.Contracts
{
    using MediatR;

    public record ReadAllWeatherRequest() : IRequest<IEnumerable<WeatherResponse>>;
    public record ReadSingleWeatherRequest(DateTime Date) : IRequest<WeatherResponse>;
}
