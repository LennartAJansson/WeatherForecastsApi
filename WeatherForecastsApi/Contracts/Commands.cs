namespace WebApplication1.Contracts
{
    using MediatR;

    public record CreateWeatherForecast() : IRequest<WeatherResponse>;
    public record UpdateWeatherForecast() : IRequest<WeatherResponse>;
    public record DeleteWeatherForecast() : IRequest<WeatherResponse>;

}
