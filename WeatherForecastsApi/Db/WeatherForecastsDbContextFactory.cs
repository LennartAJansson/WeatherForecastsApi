namespace WeatherForecastsApi.Db
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;

    public class WeatherForecastsDbContextFactory : IDesignTimeDbContextFactory<WeatherForecastsDbContext>
    {
        public WeatherForecastsDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .AddUserSecrets("e67fa587-4890-43d7-b8bf-960f9081e37d")
                //.AddUserSecrets<WeatherForecastController>()
                .Build();

            string? connectionString = configuration.GetConnectionString("WeatherForecastsDb");

            return new WeatherForecastsDbContext(new DbContextOptionsBuilder<WeatherForecastsDbContext>()
                .UseSqlServer(connectionString)
                .Options);
        }
    }
}
