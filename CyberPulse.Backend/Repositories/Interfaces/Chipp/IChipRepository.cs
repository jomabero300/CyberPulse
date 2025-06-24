using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Chipp;

public interface IChipRepository
{
    Task<ActionResponse<Chip>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Chip>>> GetAsync();

    Task<ActionResponse<Chip>> AddAsync(ChipDTO city);

    Task<ActionResponse<Chip>> UpdateAsync(ChipDTO city);

    Task<ActionResponse<Chip>> DeleteAsync(int id);

    Task<IEnumerable<Chip>> GetComboAsync(int id);

    Task<ActionResponse<IEnumerable<Chip>>> GetAsync(PaginationDTO pagination);
    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);

}
