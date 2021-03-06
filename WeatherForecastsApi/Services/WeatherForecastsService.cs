namespace WeatherForecastsApi.Services
{
    using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<WeatherForecast>?> ReadAllWeatherForecasts()
        {
            IEnumerable<WeatherForecast>? result = await context.WeatherForecasts!.ToListAsync();

            return result;
        }

        public async Task<WeatherForecast?> ReadSingleWeatherForecast(DateTime date)
        {
            //Returns forecast for the same hour. Minutes and seconds will be ignored
            WeatherForecast? result = await context.WeatherForecasts!
                .Where(w => w.Date.Year == date.Year && w.Date.Month == date.Month && w.Date.Day == date.Day && w.Date.Hour == date.Hour)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<WeatherForecast?> CreateWeatherForecast(WeatherForecast forecast)
        {
            WeatherForecast? result = await ReadSingleWeatherForecast(forecast.Date);
            if (result != null)
            {
                logger.LogInformation("WeatherForecast already exists, not added");
                return null;
            }

            context.WeatherForecasts?.Add(forecast);

            await context.SaveChangesAsync();

            logger.LogInformation("WeatherForecast added");

            return forecast;
        }

        public async Task<WeatherForecast?> UpdateWeatherForecast(WeatherForecast forecast)
        {
            WeatherForecast? weatherForecast = context.WeatherForecasts?.Find(forecast.Id);
            if (weatherForecast == null)
            {
                logger.LogInformation("WeatherForecast not found, not updated");
                return null;
            }

            weatherForecast.Date = forecast.Date;
            weatherForecast.TemperatureC = forecast.TemperatureC;
            weatherForecast.Summary = forecast.Summary;

            await context.SaveChangesAsync();

            logger.LogInformation("WeatherForecast updated");

            return weatherForecast;
        }

        public async Task<WeatherForecast?> DeleteWeatherForecast(WeatherForecast forecast)
        {
            WeatherForecast? weatherForecast = context.WeatherForecasts?.Find(forecast.Id);
            if (weatherForecast == null)
            {
                logger.LogInformation("WeatherForecast not found, not deleted");
                return null;
            }

            context.WeatherForecasts?.Remove(weatherForecast);

            await context.SaveChangesAsync();

            logger.LogInformation("WeatherForecast deleted");

            return weatherForecast;
        }
    }
}
