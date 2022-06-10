namespace WeatherForecastsApi.Db
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    using WeatherForecastsApi.Controllers;

    public class WeatherForecastsDbContextFactory : IDesignTimeDbContextFactory<WeatherForecastsDbContext>
    {
        public WeatherForecastsDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .AddUserSecrets<WeatherForecastController>()
                .Build();

            string? connectionString = configuration.GetConnectionString("WeatherForecastsDb");

            return new WeatherForecastsDbContext(new DbContextOptionsBuilder<WeatherForecastsDbContext>()
                .UseSqlServer(connectionString)
                .Options);
        }
    }
}
