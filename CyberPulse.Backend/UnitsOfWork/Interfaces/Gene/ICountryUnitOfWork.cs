using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;

public interface ICountryUnitOfWork
{
    Task<ActionResponse<Country>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Country>>> GetAsync();

    Task<ActionResponse<Country>> AddAsync(CountryDTO country);

    Task<ActionResponse<Country>> UpdateAsync(CountryDTO country);

    Task<ActionResponse<Country>> GetAsync(int id, bool lb);
    Task<ActionResponse<Country>> DeleteAsync(int id);

    Task<IEnumerable<Country>> GetComboAsync();

    Task<ActionResponse<IEnumerable<Country>>> GetAsync(PaginationDTO pagination);
    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}
