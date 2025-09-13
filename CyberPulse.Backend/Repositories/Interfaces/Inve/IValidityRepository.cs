using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Inve;

public interface IValidityRepository
{
    Task<ActionResponse<Validity>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Validity>>> GetAsync();

    Task<ActionResponse<Validity>> AddAsync(ValidityDTO entity);

    Task<ActionResponse<Validity>> UpdateAsync(ValidityDTO entity);

    Task<ActionResponse<Validity>> DeleteAsync(int id);

    Task<IEnumerable<Validity>> GetComboAsync();

    Task<ActionResponse<IEnumerable<Validity>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
    Task<ActionResponse<int>> GetNewValidityAsync();
}