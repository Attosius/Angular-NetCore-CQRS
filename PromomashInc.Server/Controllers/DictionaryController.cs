using AutoMapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromomashInc.Server.Context;
using PromomashInc.Server.Dto;
using System.Security.Cryptography;
using AutoMapper.QueryableExtensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PromomashInc.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DictionaryController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly BloggingContext _bloggingContext;
        private readonly IMapper _mapper;

        public DictionaryController(
            ILogger<UserController> logger,
            BloggingContext bloggingContext,
            IMapper mapper
            )
        {
            _logger = logger;
            _bloggingContext = bloggingContext;
            _mapper = mapper;
        }

        [HttpGet(nameof(GetCountries))]
        public IEnumerable<CountryDto> GetCountries()
        {
            _logger.LogInformation("test setse");
            var data = _bloggingContext.Countries
                .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
                .ToList();
            return data;
        }  
        
        [HttpGet(nameof(GetProvince))]
        public IEnumerable<ProvinceDto> GetProvince(string countryCode)
        {
            Thread.Sleep(1000);

            var data = _bloggingContext.Provinces
                .ProjectTo<ProvinceDto>(_mapper.ConfigurationProvider)
                .ToList();
            return data;
        }
        
    }
}
