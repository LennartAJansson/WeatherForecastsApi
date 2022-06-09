namespace WebApplication1.Mediators
{
    using MediatR;

    using System.Threading;
    using System.Threading.Tasks;

    using WebApplication1.Contracts;

    //It would be possible to simply combine CommandMediators.cs and QueryMediators.cs into a single class
    public class CommandMediators :
        IRequestHandler<CreateWeatherForecast, WeatherResponse>,
        IRequestHandler<UpdateWeatherForecast, WeatherResponse>,
        IRequestHandler<DeleteWeatherForecast, WeatherResponse>
    {
        public Task<WeatherResponse> Handle(CreateWeatherForecast request, CancellationToken cancellationToken)
        {
            //Add functionality to Create A Single Weather Request
            throw new NotImplementedException();
        }

        public Task<WeatherResponse> Handle(UpdateWeatherForecast request, CancellationToken cancellationToken)
        {
            //Add functionality to Update A Single Weather Request
            throw new NotImplementedException();
        }

        public Task<WeatherResponse> Handle(DeleteWeatherForecast request, CancellationToken cancellationToken)
        {
            //Add functionality to Delete A Single Weather Request
            throw new NotImplementedException();
        }
    }
}
