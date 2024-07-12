using PromomashInc.EntitiesDto;

namespace PromomashInc.Core.Models;

public interface IDictionaryRepository
{
    Task<List<CountryDto>> GetCountries();
    Task<List<ProvinceDto>> GetProvince(string countryCode);
}