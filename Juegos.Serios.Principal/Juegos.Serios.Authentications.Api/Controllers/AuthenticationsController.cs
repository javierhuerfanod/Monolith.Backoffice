using Microsoft.AspNetCore.Mvc;

namespace Juegos.Serios.Authentications.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationsController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<AuthenticationsController> _logger;

        public AuthenticationsController(ILogger<AuthenticationsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetAuth")]
        public IEnumerable<WeatherForecast2> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast2
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
