using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PromomashInc.Core.Models;
using PromomashInc.DataAccess.Context;
using PromomashInc.EntitiesDto;

namespace PromomashInc.Core;

public class DictionaryRepository : IDictionaryRepository
{
    private readonly UserDataContext _userDataContext;
    private readonly IMapper _mapper;

    public DictionaryRepository(
        UserDataContext userDataContext,
        IMapper mapper)
    {
        _userDataContext = userDataContext;
        _mapper = mapper;
    }

    public async Task<List<CountryDto>> GetCountries()
    {
        var data = await _userDataContext.Countries
            .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        return data;
    }

    public async Task<List<ProvinceDto>> GetProvince(string countryCode)
    {
        return await _userDataContext.Provinces
            .Where(o => o.ParentCode == countryCode)
            .ProjectTo<ProvinceDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}