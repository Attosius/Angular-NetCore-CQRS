using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromomashInc.Server.Context;
using System.Security.Cryptography;

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
            Thread.Sleep(1000);
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

        [HttpPost(nameof(Save))]
        public async ValueTask<bool> Save([FromBody] User userData)
        {
            if (userData == null)
            {
                return false;
            }
            var isNew = userData.Id == 0;
            //if (userData.Id == 0)
            //{
            //    this._bloggingContext.Users.Add(userData);
            //    await _bloggingContext.SaveChangesAsync();
            //    return true;
            //}
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: userData.PasswordHash!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            userData.PasswordHash = hashed;
            Console.WriteLine($"Hashed: {hashed}");

            var notes = _bloggingContext.Entry(userData).State = isNew ?
                EntityState.Added :
                EntityState.Modified;
            await _bloggingContext.SaveChangesAsync();

            return true;
        }
    }
}
