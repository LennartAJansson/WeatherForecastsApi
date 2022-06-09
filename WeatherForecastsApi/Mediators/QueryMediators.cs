namespace WebApplication1.Mediators
{
    using MediatR;

    using System.Threading;
    using System.Threading.Tasks;

    using WebApplication1.Contracts;

    //It would be possible to simply combine CommandMediators.cs and QueryMediators.cs into a single class
    public class QueryMediators :
        IRequestHandler<ReadAllWeatherRequest, IEnumerable<WeatherResponse>>,
        IRequestHandler<ReadSingleWeatherRequest, WeatherResponse>
    {
        public Task<IEnumerable<WeatherResponse>> Handle(ReadAllWeatherRequest request, CancellationToken cancellationToken)
        {
            //Add functionality to Read All Weather Requests
            throw new NotImplementedException();
        }

        public Task<WeatherResponse> Handle(ReadSingleWeatherRequest request, CancellationToken cancellationToken)
        {
            //Add functionality to Read A Single Weather Request
            throw new NotImplementedException();
        }
    }
}
