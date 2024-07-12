using Microsoft.AspNetCore.Mvc;
using PromomashInc.Server.Context;

namespace PromomashInc.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly BloggingContext _bloggingContext;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            BloggingContext bloggingContext
            )
        {
            _logger = logger;
            _bloggingContext = bloggingContext;
        }

        [HttpGet(nameof(GetCountries))]
        public IEnumerable<CountryDto> GetCountries()
        {
            var cou = _bloggingContext.Countries.ToList();
            return Enumerable.Range(1, 5).Select(index => new CountryDto
            {
                    Code = $"Country_{index}",
                    DisplayText = $"Country {index} "
                })
                .ToArray();
        }  
        
        [HttpGet(nameof(GetProvince))]
        public IEnumerable<ProvinceDto> GetProvince(string countryCode)
        {
            Thread.Sleep(3000);
            return Enumerable.Range(1, 5).Select(index => new ProvinceDto()
            {
                    Code = $"Province_{index}",
                    ParentCode = $"Country_{index%2 + 1}",
                    DisplayText = $"Province {index} "
                }).Where(o => o.ParentCode == countryCode)
                .ToArray();
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
