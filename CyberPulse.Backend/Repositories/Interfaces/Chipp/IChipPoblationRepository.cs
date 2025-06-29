using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Chipp;

public interface IChipPoblationRepository
{
    Task<ActionResponse<ChipPoblation>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<ChipPoblation>>> GetAsync();

    Task<ActionResponse<ChipPoblation>> AddAsync(ChipPoblationDTO entity);

    Task<ActionResponse<ChipPoblation>> DeleteAsync(int id);

    Task<IEnumerable<ChipPoblation>> GetComboAsync();

    Task<ActionResponse<IEnumerable<ChipPoblation>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);

}
