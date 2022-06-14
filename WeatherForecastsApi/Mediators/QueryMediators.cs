namespace WeatherForecastsApi.Mediators
{
    using MediatR;

    using System.Threading;
    using System.Threading.Tasks;

    using WeatherForecastsApi.Contracts;
    using WeatherForecastsApi.Model;
    using WeatherForecastsApi.Services;

    //It would be possible to simply combine CommandMediators.cs and QueryMediators.cs into a single class
    public class QueryMediators :
    IRequestHandler<ReadAllWeatherRequest, IEnumerable<WeatherResponse>?>,
    IRequestHandler<ReadSingleWeatherRequest, WeatherResponse?>
    {
        private readonly ILogger<QueryMediators> logger;
        private readonly IWeatherForecastsService service;

        public QueryMediators(ILogger<QueryMediators> logger, IWeatherForecastsService service)
        {
            this.logger = logger;
            this.service = service;
        }

        public async Task<IEnumerable<WeatherResponse>?> Handle(ReadAllWeatherRequest request, CancellationToken cancellationToken)
        {
            //Add functionality to Read All Weather Requests
            IEnumerable<WeatherForecast>? result = await service.ReadAllWeatherForecasts();

            if (result == null)
            {
                return null;
            }

            return result.Select(weatherForecast => new WeatherResponse(weatherForecast.Id, weatherForecast.Date, weatherForecast.TemperatureC, weatherForecast.TemperatureF, weatherForecast.Summary));
        }

        public async Task<WeatherResponse?> Handle(ReadSingleWeatherRequest request, CancellationToken cancellationToken)
        {
            //Add functionality to Read A Single Weather Request
            WeatherForecast? result = await service.ReadSingleWeatherForecast(request.Date);

            if (result == null)
            {
                return null;
            }

            return new WeatherResponse(result.Id, result.Date, result.TemperatureC, result.TemperatureF, result.Summary);
        }
    }
}
