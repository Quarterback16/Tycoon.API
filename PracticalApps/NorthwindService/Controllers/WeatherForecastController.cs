using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace NorthwindService.Controllers
{
    [ApiController]  // enables REST-specific behavior for controllers, like automatic HTTP 400 responses for invalid models
    [Route("[controller]")]
    //[Route("[api/forecast]")]
    public class WeatherForecastController : ControllerBase  //ControllerBase simpler than Controller as does not have methods like View to generate HTML responses using a Razor file
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{days:int}")]
        public IEnumerable<WeatherForecast> Get(int days)
        {
            var rng = new Random();
            return Enumerable.Range(1, days)
                .Select(index => new WeatherForecast
                    {
                        Date = DateTime.Now.AddDays(index),
                        TemperatureC = rng.Next(-20, 55),
                        Summary = Summaries[rng.Next(Summaries.Length)]
                    })
                .ToArray();
        }

        // GET /weatherforecast
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Get(5);
        }

    }
}
