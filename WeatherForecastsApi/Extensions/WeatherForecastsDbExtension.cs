namespace WeatherForecastsApi.Extensions
{
    using Microsoft.EntityFrameworkCore;

    using WeatherForecastsApi.Db;
    using WeatherForecastsApi.Services;

    public static class WeatherForecastsDbExtension
    {
        public static IServiceCollection AddWeatherForecastsDb(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IWeatherForecastsService, WeatherForecastsService>();
            services.AddDbContext<IWeatherForecastsDbContext, WeatherForecastsDbContext>(options => options
                    .UseSqlServer(connectionString), ServiceLifetime.Transient, ServiceLifetime.Transient);

            return services;
        }
    }
}
