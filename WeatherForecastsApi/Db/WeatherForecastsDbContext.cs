﻿namespace WeatherForecastsApi.Db
{
    using Microsoft.EntityFrameworkCore;

    using System.Text.Json;
    using System.Threading.Tasks;

    using WeatherForecastsApi.Model;

    public class WeatherForecastsDbContext : DbContext, IWeatherForecastsDbContext
    {
        private static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        protected ILogger<WeatherForecastsDbContext> logger = loggerFactory.CreateLogger<WeatherForecastsDbContext>();

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }

        public WeatherForecastsDbContext(DbContextOptions<WeatherForecastsDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.UseLoggerFactory(loggerFactory);
        }

        public async Task EnsureExists(string? seedFileName = null)
        {
            IEnumerable<string> migrations = Database.GetPendingMigrations();
            if (migrations.Any())
            {
                logger?.LogInformation("Adding {count} migrations", migrations.Count());
                await Database.MigrateAsync();


                if (seedFileName != null)
                {
                    if (File.Exists(seedFileName))
                    {
                        IEnumerable<WeatherForecast> forecasts = JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(File.ReadAllText(seedFileName));
                        await WeatherForecasts.AddRangeAsync(forecasts);
                        await SaveChangesAsync();
                    }
                    //Read file and seed data from the file
                }
            }
            else
            {
                logger?.LogInformation("Migrations are up to date");
            }
        }
    }
}
