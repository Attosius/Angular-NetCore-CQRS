using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using PromomashInc.DataAccess.Context;
using PromomashInc.EntitiesDto;
using PromomashInc.Helpers.FunctionalResult;


namespace PromomashInc.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DictionaryController : ControllerBase
    {

        private readonly ILogger<DictionaryController> _logger;
        private readonly UserDataContext _userDataContext;
        private readonly IMapper _mapper;

        public DictionaryController(
            ILogger<DictionaryController> logger,
            UserDataContext userDataContext,
            IMapper mapper
            )
        {
            _logger = logger;
            _userDataContext = userDataContext;
            _mapper = mapper;
        }

        [HttpGet(nameof(GetCountries))]
        public async Task<Result<List<CountryDto>>> GetCountries()
        {
            var result = await TryCatchExecuterAsync(async () =>
            {
                var data = await _userDataContext.Countries
                    .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                return data;
            });
            return result;
        }  
        
        [HttpGet(nameof(GetProvince))]
        public async Task<Result<List<ProvinceDto>>> GetProvince(string countryCode)
        {
            Thread.Sleep(1000);
            var result = await TryCatchExecuterAsync(async () =>
            {
                var data = await _userDataContext.Provinces
                    .ProjectTo<ProvinceDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
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
