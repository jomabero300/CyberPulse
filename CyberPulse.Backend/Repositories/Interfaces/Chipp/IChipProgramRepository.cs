using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Chipp;

public interface IChipProgramRepository
{
    Task<ActionResponse<ChipProgram>> GetAsync(string code);

    Task<IEnumerable<ChipProgram>> GetComboAsync(int id);

    Task<ActionResponse<IEnumerable<ChipProgram>>> GetAsync(PaginationDTO pagination);
    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}
