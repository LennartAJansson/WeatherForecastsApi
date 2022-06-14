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
Add classes (or a single class, think "Single Responsibility", the requesthandlers should have the same responsibility area) for your QueryMediators and CommandMediators, they should implement the interface IRequestHandler for each request type returning its responsetype:  

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
In appsettings.json add following configuration:  
```
"ConnectionStrings": {
  "WeatherForecastsDb": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WeatherForecastsDb;Integrated Security=True;"
}
``` 
## Step 7  
Move WeatherForecast.cs from the project root into the folder named Model, make a small change to it so it looks like this:
```
public class WeatherForecast
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}
```
## Step 8  
When we are at it creating a model we could update our records in the folder Contracts to reflect the model.
Update Commands.cs to:
```
public record CreateWeatherForecast(DateTime Date, double TemperatureC, string? Summary) : IRequest<WeatherResponse>;
public record UpdateWeatherForecast(int Id, DateTime Date, double TemperatureC, string? Summary) : IRequest<WeatherResponse>;
public record DeleteWeatherForecast(int Id) : IRequest<WeatherResponse>;
```
Update Responses.cs to:
```
public record WeatherResponse(int Id, DateTime Date, double TemperatureC, double TemperatureF, string? Summary);
```
## Step 9  
Add one interface and two classes to the folder Db:  
IWeatherForecastsDbContext.cs:  
```
public interface IWeatherForecastsDbContext
{
    DbSet<WeatherForecast>? WeatherForecasts { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    Task EnsureExists(string? seedFileName = null);
}
```
WeatherForecastsDbContext.cs:  
```
public class WeatherForecastsDbContext : DbContext, IWeatherForecastsDbContext
{
    private static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
    protected ILogger<WeatherForecastsDbContext> logger = loggerFactory.CreateLogger<WeatherForecastsDbContext>();

    public DbSet<WeatherForecast>? WeatherForecasts { get; set; }

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
        }
        else
        {
            logger?.LogInformation("Migrations are up to date");
        }

        if (WeatherForecasts != null && !WeatherForecasts.Any())
        {
            if (seedFileName != null && File.Exists(seedFileName))
            {
                IEnumerable<WeatherForecast>? forecasts = JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(File.ReadAllText(seedFileName));
                if (forecasts != null && forecasts.Any())
                {
                    logger?.LogInformation("Adding {count} seeded data", forecasts.Count());
                    await WeatherForecasts.AddRangeAsync(forecasts);
                    await SaveChangesAsync();
                }
            }
        }
    }
}
```
WeatherForecastsDbContextFactory.cs:  
```
public class WeatherForecastsDbContextFactory : IDesignTimeDbContextFactory<WeatherForecastsDbContext>
{
    public WeatherForecastsDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            //Either add the UserSecretsId from your .csproj
            .AddUserSecrets("e67fa587-4890-43d7-b8bf-960f9081e37d")
            //Or you could use the typed version of AddUserSecrets
            //.AddUserSecrets<WeatherForecastController>()
            .Build();

        string? connectionString = configuration.GetConnectionString("WeatherForecastsDb");

        return new WeatherForecastsDbContext(new DbContextOptionsBuilder<WeatherForecastsDbContext>()
            .UseSqlServer(connectionString)
            .Options);
    }
}
```
## Step 10  
Add a folder named Services and add following interface and class to it.  
IWeatherForecastsService.cs:  
```
public interface IWeatherForecastsService
{
    Task<IEnumerable<WeatherForecast>?> ReadAllWeatherForecasts();
    Task<WeatherForecast?> ReadSingleWeatherForecast(DateTime date);
    Task<WeatherForecast?> CreateWeatherForecast(WeatherForecast forecast);
    Task<WeatherForecast?> UpdateWeatherForecast(WeatherForecast forecast);
    Task<WeatherForecast?> DeleteWeatherForecast(WeatherForecast forecast);
}
```
WeatherForecastsService.cs  
```
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
```
## Step 11  
Add a folder named Extensions and add following extension class to it.  
WeatherForecastsDbExtension.cs  
```
public static class WeatherForecastsDbExtension
{
    public static IServiceCollection AddWeatherForecastsDb(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IWeatherForecastsService, WeatherForecastsService>();
        services.AddDbContext<IWeatherForecastsDbContext, WeatherForecastsDbContext>(options => options
                .UseSqlServer(connectionString), ServiceLifetime.Transient, ServiceLifetime.Transient);

        return services;
    }

    public static WebApplication UpdateDatabase(this WebApplication application, string seedFileName)
    {
        IWeatherForecastsDbContext ctx = application.Services.GetRequiredService<IWeatherForecastsDbContext>();
        ctx.EnsureExists(seedFileName);
        return application;
    }
}
```  
## Step 12  
Add following line to Program.cs (before the line with AddMediatR)  
```
builder.Services.AddWeatherForecastsDb(builder.Configuration.GetConnectionString("WeatherForecastsDb"));
```
After the line with builder.Build(), in the same file, add:
```
app.UpdateDatabase(@".\seed.json");
```
## Step 13  
Update CommandMediator.cs:  
```
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
```
Update QueryMediator.cs:  
```
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

        return result.Select(r => new WeatherResponse(r.Id, r.Date, r.TemperatureC, r.TemperatureF, r.Summary));
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
```
## Step 14  
Open up a Package manager console and in that console write following commands:
```
Add-Migration Initial
Update-Database
```
## Step 15  
In the project rootfolder add a file named seed.json, it should contain some sample data in the following shape (add as many you want):  
```
[
  {
    "Date": "2022-06-01T00:00:00",
    "TemperatureC": 10,
    "Summary": "Summer"
  },
  {
    "Date": "2022-06-01T01:00:00",
    "TemperatureC": 11,
    "Summary": "Summer"
  },
  {
    "Date": "2022-06-01T02:00:00",
    "TemperatureC": 12,
    "Summary": "Summer"
  },
  {
    "Date": "2022-06-01T03:00:00",
    "TemperatureC": 13,
    "Summary": "Summer"
  },
  {
    "Date": "2022-06-01T04:00:00",
    "TemperatureC": 14,
    "Summary": "Summer"
  },
  {
    "Date": "2022-06-01T05:00:00",
    "TemperatureC": 15,
    "Summary": "Summer"
  },
  {
    "Date": "2022-06-01T06:00:00",
    "TemperatureC": 16,
    "Summary": "Summer"
  }
]
```
