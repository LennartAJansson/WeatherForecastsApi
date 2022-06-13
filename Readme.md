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
## Step 9  
Add one interface and two classes to the folder Db:  
IWeatherForecastsDbContext.cs:  
```
public interface IWeatherForecastsDbContext
{
    DbSet<WeatherForecast> WeatherForecasts { get; set; }
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
                //Read file and seed data from the file
            }
        }
        else
        {
            logger?.LogInformation("Migrations are up to date");
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
            .AddUserSecrets<WeatherForecastController>()
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
    Task<IEnumerable<WeatherForecast>> ReadAllWeatherForecasts();
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
        context.EnsureExists().Wait();
    }

    public Task<IEnumerable<WeatherForecast>> ReadAllWeatherForecasts()
    {
        return Task.FromResult(context.WeatherForecasts.AsEnumerable());
    }

    public Task<WeatherForecast?> ReadSingleWeatherForecast(DateTime date)
    {

        return Task.FromResult(context.WeatherForecasts.FirstOrDefault(w => date.Subtract(w.Date).TotalHours == 1 || w.Date.Subtract(date).TotalHours == 1));
    }

    public async Task<WeatherForecast?> CreateWeatherForecast(WeatherForecast forecast)
    {
        if (await ReadSingleWeatherForecast(forecast.Date) != null)
        {
            return null;
        }
        context.WeatherForecasts.Add(forecast);
        await context.SaveChangesAsync();
        return forecast;
    }

    public async Task<WeatherForecast?> UpdateWeatherForecast(WeatherForecast forecast)
    {
        if (context.WeatherForecasts.Find(forecast.Id) == null)
        {
            return null;
        }
        context.WeatherForecasts.Update(forecast);
        await context.SaveChangesAsync();
        return forecast;
    }

    public async Task<WeatherForecast?> DeleteWeatherForecast(WeatherForecast forecast)
    {
        if (context.WeatherForecasts.Find(forecast.Id) == null)
        {
            return null;
        }
        context.WeatherForecasts.Remove(forecast);
        await context.SaveChangesAsync();
        return forecast;
    }
}
```
## Step 11  
Add a folder named Extensions and add following extension class to it.  
WeatherForecastsDbExtension.cs  
```
public static class WeatherForecastsDbExtension
{
    public static IServiceCollection AddWeatherForecastsDb(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddTransient<IWeatherForecastsService, WeatherForecastsService>()
            .AddDbContext<WeatherForecastsDbContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("WeatherForecastsDb")),
                    ServiceLifetime.Transient, ServiceLifetime.Transient);
    }
}
```  
## Step 12  
Add following line to Program.cs (after the line with AddMediatR)  
```
builder.Services.AddWeatherForecastsDb(builder.Configuration.GetConnectionString("WeatherForecastsDb"));
```
## Step 13  
Add following fields and constructor to CommandMediator.cs:  
```
private readonly ILogger<CommandMediators> logger;
private readonly IWeatherForecastsService service;

public CommandMediators(ILogger<CommandMediators> logger, IWeatherForecastsService service)
{
    this.logger = logger;
    this.service = service;
}
```
Add following fields and constructor to QueryMediator.cs:  
```
private readonly ILogger<QueryMediators> logger;
private readonly IWeatherForecastsService service;

public QueryMediators(ILogger<QueryMediators> logger, IWeatherForecastsService service)
{
    this.logger = logger;
    this.service = service;
}
```
## Step 14  
Add methods to the mediators...  
## Step 15  
Open up a Package manager console and in that console write following commands:
```
Add-Migration Initial
Update-Database
```
