using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.EntitiesDTO.Chipp.Report;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Chipp;

public interface IChipRepository
{
    Task<ActionResponse<Chip>> GetAsync(int id);
    Task<ActionResponse<Chip>> GetAsync(ChipReportDTO entity);
    Task<ActionResponse<IEnumerable<Chip>>> GetAsync(ChipReport entity);
    Task<ActionResponse<IEnumerable<Chip>>> GetAsync(DateTime date);
    Task<ActionResponse<IEnumerable<Chip>>> GetAsync();

    Task<ActionResponse<Chip>> AddAsync(ChipDTO entity);

    Task<ActionResponse<Chip>> UpdateAsync(ChipDTO entity);
    Task<ActionResponse<Chip>> UpdateAsync(ChipCoordinator entity);

    Task<ActionResponse<Chip>> DeleteAsync(int id);

    Task<ActionResponse<IEnumerable<Chip>>> GetAsync(PaginationDTO pagination);
    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);

}
