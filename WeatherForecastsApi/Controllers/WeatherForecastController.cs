namespace WeatherForecastsApi.Controllers;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using WeatherForecastsApi.Contracts;

[ApiController]
[Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> logger;
    private readonly IMediator mediator;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WeatherResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WeatherResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(WeatherResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateOne([FromBody] CreateWeatherForecast request)
    {
        WeatherResponse? response = await mediator.Send(request);

        if (response == null)
        {
            return NotFound();
        }
        else
        {
            return Created("", response);
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(WeatherResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOne([FromBody] UpdateWeatherForecast request)
    {
        WeatherResponse? response = await mediator.Send(request);

        if (response == null)
        {
            return NotFound();
        }
        else
        {
            return Accepted(response);
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(WeatherResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOne([FromBody] DeleteWeatherForecast request)
    {
        WeatherResponse? response = await mediator.Send(request);

        if (response == null)
        {
            return NotFound();
        }
        else
        {
            return Accepted(response);
        }
    }
}