namespace WebApplication1.Controllers
{
    using MediatR;

    using Microsoft.AspNetCore.Mvc;

    using WebApplication1.Contracts;

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
}