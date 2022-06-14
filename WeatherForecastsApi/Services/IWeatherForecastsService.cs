namespace WeatherForecastsApi.Services
{
    using WeatherForecastsApi.Model;

    public interface IWeatherForecastsService
    {
        Task<IEnumerable<WeatherForecast>?> ReadAllWeatherForecasts();
        Task<WeatherForecast?> ReadSingleWeatherForecast(DateTime date);
        Task<WeatherForecast?> CreateWeatherForecast(WeatherForecast forecast);
        Task<WeatherForecast?> UpdateWeatherForecast(WeatherForecast forecast);
        Task<WeatherForecast?> DeleteWeatherForecast(WeatherForecast forecast);
    }
}
