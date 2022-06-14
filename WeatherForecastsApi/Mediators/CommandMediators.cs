namespace WeatherForecastsApi.Mediators
{
    using MediatR;

    using System.Threading;
    using System.Threading.Tasks;

    using WeatherForecastsApi.Contracts;
    using WeatherForecastsApi.Model;
    using WeatherForecastsApi.Services;

    //It would be possible to simply combine CommandMediators.cs and QueryMediators.cs into a single class
    public class CommandMediators :
        IRequestHandler<CreateWeatherForecast, WeatherResponse?>,
        IRequestHandler<UpdateWeatherForecast, WeatherResponse?>,
        IRequestHandler<DeleteWeatherForecast, WeatherResponse?>
    {
        private readonly ILogger<CommandMediators> logger;
        private readonly IWeatherForecastsService service;

        public CommandMediators(ILogger<CommandMediators> logger, IWeatherForecastsService service)
        {
            this.logger = logger;
            this.service = service;
        }

        public async Task<WeatherResponse?> Handle(CreateWeatherForecast request, CancellationToken cancellationToken)
        {
            //Add functionality to Create A Single Weather Request
            WeatherForecast forecast = new WeatherForecast { Date = request.Date, TemperatureC = request.TemperatureC, Summary = request.Summary };

            WeatherForecast? result = await service.CreateWeatherForecast(forecast);

            if (result == null)
            {
                return null;
            }

            return new WeatherResponse(result.Id, result.Date, result.TemperatureC, result.TemperatureF, result.Summary);
        }

        public async Task<WeatherResponse?> Handle(UpdateWeatherForecast request, CancellationToken cancellationToken)
        {
            //Add functionality to Update A Single Weather Request
            WeatherForecast forecast = new WeatherForecast { Id = request.Id, Date = request.Date, TemperatureC = request.TemperatureC, Summary = request.Summary };

            WeatherForecast? result = await service.UpdateWeatherForecast(forecast);

            if (result == null)
            {
                return null;
            }

            return new WeatherResponse(result.Id, result.Date, result.TemperatureC, result.TemperatureF, result.Summary);
        }

        public async Task<WeatherResponse?> Handle(DeleteWeatherForecast request, CancellationToken cancellationToken)
        {
            //Add functionality to Delete A Single Weather Request
            WeatherForecast forecast = new WeatherForecast { Id = request.Id };

            WeatherForecast? result = await service.DeleteWeatherForecast(forecast);

            if (result == null)
            {
                return null;
            }

            return new WeatherResponse(result.Id, result.Date, result.TemperatureC, result.TemperatureF, result.Summary);
        }
    }
}
