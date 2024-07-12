using AutoMapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromomashInc.Server.Context;
using PromomashInc.Server.Dto;
using System.Security.Cryptography;
using AutoMapper.QueryableExtensions;
using Helpers.FunctionalResult;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PromomashInc.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DictionaryController : ControllerBase
    {

        private readonly ILogger<DictionaryController> _logger;
        private readonly BloggingContext _bloggingContext;
        private readonly IMapper _mapper;

        public DictionaryController(
            ILogger<DictionaryController> logger,
            BloggingContext bloggingContext,
            IMapper mapper
            )
        {
            _logger = logger;
            _bloggingContext = bloggingContext;
            _mapper = mapper;
        }

        [HttpGet(nameof(GetCountries))]
        public async Task<Result<List<CountryDto>>> GetCountries()
        {
            var result = await TryCatchExecuterAsync(async () =>
            {
                var data = await _bloggingContext.Countries
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
                var data = await _bloggingContext.Provinces
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
