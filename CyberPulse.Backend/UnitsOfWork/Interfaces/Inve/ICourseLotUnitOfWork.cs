using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;

public interface ICourseLotUnitOfWork
{
    Task<ActionResponse<CourseLot>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<CourseLot>>> GetAsync();

    Task<ActionResponse<CourseLot>> AddAsync(CourseLotDTO entity);

    Task<ActionResponse<CourseLot>> UpdateAsync(CourseLotDTO entity);

    Task<ActionResponse<CourseLot>> DeleteAsync(int id);

    Task<IEnumerable<CourseLot>> GetComboAsync(int id);

    Task<ActionResponse<IEnumerable<CourseLot>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}