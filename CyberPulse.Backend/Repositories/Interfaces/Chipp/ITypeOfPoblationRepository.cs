using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Chipp;

public interface ITypeOfPoblationRepository
{
    Task<ActionResponse<TypeOfPoblation>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<TypeOfPoblation>>> GetAsync();
    Task<IEnumerable<TypeOfPoblationDTO>> GetAsync(string filter);
    Task<ActionResponse<TypeOfPoblation>> AddAsync(TypeOfPoblation entity);
    Task<ActionResponse<TypeOfPoblation>> DeleteAsync(int id);
    Task<IEnumerable<TypeOfPoblation>> GetComboAsync();
    Task<ActionResponse<IEnumerable<TypeOfPoblation>>> GetAsync(PaginationDTO pagination);
    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}
