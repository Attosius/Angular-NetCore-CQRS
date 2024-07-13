using Microsoft.Extensions.Caching.Memory;
using PromomashInc.Core.Models;
using PromomashInc.EntitiesDto;

namespace PromomashInc.Core;

public class CachedDictionaryRepository : ICachedDictionaryRepository
{
    private readonly IMemoryCache _memoryCache;
    private readonly IDictionaryRepository _dictionaryRepository;

    public CachedDictionaryRepository(
        IMemoryCache memoryCache, // redis, apache ignite, etc
        IDictionaryRepository dictionaryRepository)
    {
        _memoryCache = memoryCache;
        _dictionaryRepository = dictionaryRepository;
    }

    public async Task<List<CountryDto>> GetCountries()
    {
        var key = nameof(CountryDto);
        if (_memoryCache.TryGetValue(key, out List<CountryDto> cachedData))
        {
            return cachedData;
        }
        cachedData = await _dictionaryRepository.GetCountries();
        _memoryCache.Set(key, cachedData, TimeSpan.FromMinutes(30));
        return cachedData;
    }

    public async Task<List<ProvinceDto>> GetProvince(string countryCode)
    {
        var key = $"{nameof(GetProvince)}_{countryCode}";
        if (_memoryCache.TryGetValue(key, out List<ProvinceDto> cachedData))
        {
            return cachedData;
        }
        cachedData = await _dictionaryRepository.GetProvince(countryCode);
        _memoryCache.Set(key, cachedData, TimeSpan.FromMinutes(1));
        return cachedData;
    }
}