using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Inve;

public interface IProgramLotRepository
{
    Task<ActionResponse<ProgramLot>> GetAsync(int id);
    Task<ActionResponse<IEnumerable<ProgramLot>>> GetAsync();
    Task<ActionResponse<ProgramLot>> AddAsync(ProgramLotDTO entity);
    Task<ActionResponse<ProgramLot>> UpdateAsync(ProgramLotDTO entity);
    Task<ActionResponse<ProgramLot>> DeleteAsync(int id);
    Task<IEnumerable<ProgramLot>> GetComboAsync(int id);
    Task<ActionResponse<IEnumerable<ProgramLot>>> GetAsync(PaginationDTO pagination);
    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}