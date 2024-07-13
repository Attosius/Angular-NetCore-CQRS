using Microsoft.AspNetCore.Mvc;
using PromomashInc.EntitiesDto;
using PromomashInc.Helpers.FunctionalResult;
using PromomashInc.Core;


namespace PromomashInc.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DictionaryController : ControllerBase
    {

        private readonly ILogger<DictionaryController> _logger;
        private readonly ICachedDictionaryRepository _dictionaryRepository;

        public DictionaryController(
            ILogger<DictionaryController> logger,
            ICachedDictionaryRepository dictionaryRepository
            )
        {
            _logger = logger;
            _dictionaryRepository = dictionaryRepository;
        }

        [HttpGet(nameof(GetCountries))]
        public async Task<Result<List<CountryDto>>> GetCountries()
        {
            var result = await TryCatchExecuterAsync(async () => await _dictionaryRepository.GetCountries());
            return result;
        }  
        
        [HttpGet(nameof(GetProvince))]
        public async Task<Result<List<ProvinceDto>>> GetProvince(string countryCode)
        {
            Thread.Sleep(1000); // for loader
            var result = await TryCatchExecuterAsync(async () =>
            {
                var data = await _dictionaryRepository.GetProvince(countryCode);
                return data;
            });

            return result;
        }

        private async Task<Result<T>> TryCatchExecuterAsync<T>(Func<Task<T>> func)
        {
            try
            {
                var data = await func();
                return data.ToSuccessResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Results error");
                return e.ToErrorResult<T>();
            }
        }
    }
}
