using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Gene;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Gene;

public class CountryUnitOfWork : GenericUnitOfWork<Country>, ICountryUnitOfWork
{
    private readonly ICountryRepository _countryRepository;

    public CountryUnitOfWork(IGenericRepository<Country> repository, ICountryRepository countryRepository) : base(repository)
    {
        _countryRepository = countryRepository;
    }

    public async Task<ActionResponse<Country>> AddAsync(CountryDTO country)=>await _countryRepository.AddAsync(country);

    public async Task<IEnumerable<Country>> GetComboAsync()=>await _countryRepository.GetComboAsync();

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _countryRepository.GetTotalRecordsAsync(pagination);

    public async Task<ActionResponse<Country>> UpdateAsync(CountryDTO country) => await _countryRepository.UpdateAsync(country);

}
