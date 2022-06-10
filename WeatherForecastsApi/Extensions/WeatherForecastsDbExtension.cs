namespace WeatherForecastsApi.Extensions
{
    using Microsoft.EntityFrameworkCore;

    using WeatherForecastsApi.Db;
    using WeatherForecastsApi.Services;

    public static class WeatherForecastsDbExtension
    {
        public static IServiceCollection AddWeatherForecastsDb(this IServiceCollection services, string connectionString)
        {
            return services
                .AddTransient<IWeatherForecastsService, WeatherForecastsService>()
                .AddDbContext<WeatherForecastsDbContext>(options => options
                    .UseSqlServer(connectionString),
                        ServiceLifetime.Transient, ServiceLifetime.Transient);
        }
    }
}
