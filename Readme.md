## Step 1  
Add a reference to 'MediatR.Extensions.Microsoft.DependencyInjection'  
## Step 2  
Create a folder named Contracts  
Add records for request data types  
Add records for response data types  
Connect request data type to response data type by adding the interface IRequest of response datatype:  

```
public record ReadAllWeatherRequest() : IRequest<IEnumerable<WeatherResponse>>;
public record ReadSingleWeatherRequest(DateTime Date) : IRequest<WeatherResponse>; 
public record CreateWeatherForecast() : IRequest<WeatherResponse>;
public record UpdateWeatherForecast() : IRequest<WeatherResponse>;
public record DeleteWeatherForecast() : IRequest<WeatherResponse>;
public record WeatherResponse(DateTime Date, double TempC, double TempF);
```

## Step 3
Create a folder named Mediators  
Add classes for QueryMediators and CommandQueryMediators, they should implement the interface IRequestHandler for each request type returning its responsetype:  

```
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
```

```
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
```
  
## Step 4  
Update the controller to look like this:
```
[ApiController]
[Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IMediator mediator;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
    {
        _logger = logger;
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        ReadAllWeatherRequest request = new ReadAllWeatherRequest();

        IEnumerable<WeatherResponse>? response = await mediator.Send(request);

        if (response == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(response);
        }
    }

    [HttpGet("{date}")]
    public async Task<IActionResult> GetOne(DateTime date)
    {
        ReadSingleWeatherRequest request = new ReadSingleWeatherRequest(date);

        WeatherResponse? response = await mediator.Send(request);

        if (response == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(response);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOne([FromBody] CreateWeatherForecast request)
    {
        WeatherResponse? response = await mediator.Send(request);

        if (response == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(response);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateOne([FromBody] UpdateWeatherForecast request)
    {
        WeatherResponse? response = await mediator.Send(request);

        if (response == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(response);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteOne([FromBody] DeleteWeatherForecast request)
    {
        WeatherResponse? response = await mediator.Send(request);

        if (response == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(response);
        }
    }
}
```

## Step 5  
In program.cs add following lines after "//Add services to the container"

```
// Add services to the container.
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(WeatherForecastController))
    ?? throw new ArgumentNullException("Couldn't find assembly"));
```

## Step 6  
## Step 7  
## Step 9  
## Step 10  
