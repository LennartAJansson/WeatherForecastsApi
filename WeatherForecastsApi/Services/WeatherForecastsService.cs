namespace WeatherForecastsApi.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WeatherForecastsApi.Db;
    using WeatherForecastsApi.Model;

    public class WeatherForecastsService : IWeatherForecastsService
    {
        private readonly ILogger<WeatherForecastsService> logger;
        private readonly IWeatherForecastsDbContext context;

        public WeatherForecastsService(ILogger<WeatherForecastsService> logger, IWeatherForecastsDbContext context)
        {
            this.logger = logger;
            this.context = context;
            context.EnsureExists().Wait();
        }

        public Task<IEnumerable<WeatherForecast>> ReadAllWeatherForecasts()
        {
            return Task.FromResult(context.WeatherForecasts.AsEnumerable());
        }

        public Task<WeatherForecast?> ReadSingleWeatherForecast(DateTime date)
        {

            return Task.FromResult(context.WeatherForecasts.FirstOrDefault(w => date.Subtract(w.Date).TotalHours == 1 || w.Date.Subtract(date).TotalHours == 1));
        }

        public async Task<WeatherForecast?> CreateWeatherForecast(WeatherForecast forecast)
        {
            if (await ReadSingleWeatherForecast(forecast.Date) != null)
            {
                return null;
            }
            context.WeatherForecasts.Add(forecast);
            await context.SaveChangesAsync();
            return forecast;
        }

        public async Task<WeatherForecast?> UpdateWeatherForecast(WeatherForecast forecast)
        {
            if (context.WeatherForecasts.Find(forecast.Id) == null)
            {
                return null;
            }
            context.WeatherForecasts.Update(forecast);
            await context.SaveChangesAsync();
            return forecast;
        }

        public async Task<WeatherForecast?> DeleteWeatherForecast(WeatherForecast forecast)
        {
            if (context.WeatherForecasts.Find(forecast.Id) == null)
            {
                return null;
            }
            context.WeatherForecasts.Remove(forecast);
            await context.SaveChangesAsync();
            return forecast;
        }
    }
}
