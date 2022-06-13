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
        }

        public Task<IEnumerable<WeatherForecast>?> ReadAllWeatherForecasts()
        {
            IEnumerable<WeatherForecast>? result = context.WeatherForecasts?.AsEnumerable();
            return Task.FromResult(result);
        }

        public Task<WeatherForecast?> ReadSingleWeatherForecast(DateTime date)
        {
            //Returns forecast for the same hour. Minutes and seconds will be ignored
            WeatherForecast? result = context.WeatherForecasts?
                .Where(w => w.Date.Year == date.Year && w.Date.Month == date.Month && w.Date.Day == date.Day && w.Date.Hour == date.Hour)
                .FirstOrDefault();

            return Task.FromResult(result);
        }

        public async Task<WeatherForecast?> CreateWeatherForecast(WeatherForecast forecast)
        {
            if (await ReadSingleWeatherForecast(forecast.Date) != null)
            {
                return null;
            }

            context.WeatherForecasts?.Add(forecast);

            await context.SaveChangesAsync();

            return forecast;
        }

        public async Task<WeatherForecast?> UpdateWeatherForecast(WeatherForecast forecast)
        {
            WeatherForecast? weatherForecast = context.WeatherForecasts?.Find(forecast.Id);
            if (weatherForecast == null)
            {
                return null;
            }

            weatherForecast.Date = forecast.Date;
            weatherForecast.TemperatureC = forecast.TemperatureC;
            weatherForecast.Summary = forecast.Summary;

            //context.WeatherForecasts.Update()

            await context.SaveChangesAsync();

            return forecast;
        }

        public async Task<WeatherForecast?> DeleteWeatherForecast(WeatherForecast forecast)
        {
            WeatherForecast? weatherForecast = context.WeatherForecasts?.Find(forecast.Id);
            if (weatherForecast == null)
            {
                return null;
            }

            context.WeatherForecasts?.Remove(weatherForecast);

            await context.SaveChangesAsync();

            return weatherForecast;
        }
    }
}
