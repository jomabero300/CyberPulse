using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;

public interface IFamilyUnitOfWork
{
    Task<ActionResponse<Family>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Family>>> GetAsync();
    Task<ActionResponse<IEnumerable<Family>>> GetAsync(string Filter);
    Task<ActionResponse<Family>> AddAsync(FamilyDTO entity);

    Task<ActionResponse<Family>> UpdateAsync(FamilyDTO entity);

    Task<ActionResponse<Family>> DeleteAsync(int id);

    Task<IEnumerable<Family>> GetComboAsync(int id);

    Task<ActionResponse<IEnumerable<Family>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}