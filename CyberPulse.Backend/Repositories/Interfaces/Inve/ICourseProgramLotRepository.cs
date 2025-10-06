using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Inve;

public interface ICourseProgramLotRepository
{
    Task<ActionResponse<CourseProgramLot>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<CourseProgramLot>>> GetAsync();

    Task<ActionResponse<CourseProgramLot>> AddAsync(CourseProgramLotDTO entity);

    Task<ActionResponse<CourseProgramLot>> UpdateAsync(CourseProgramLotDTO entity);

    Task<ActionResponse<CourseProgramLot>> DeleteAsync(int id);

    Task<IEnumerable<CourseProgramLot>> GetComboAsync(int id);

    Task<ActionResponse<IEnumerable<CourseProgramLot>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}