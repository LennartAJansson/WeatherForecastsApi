namespace WeatherForecastsApi.Db
{
    using Microsoft.EntityFrameworkCore;

    using WeatherForecastsApi.Model;

    public interface IWeatherForecastsDbContext
    {
        DbSet<WeatherForecast>? WeatherForecasts { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task EnsureExists(string? seedFileName = null);
    }
}
