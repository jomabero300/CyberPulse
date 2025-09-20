using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Inve;

public interface ICourseRepository
{
    Task<ActionResponse<Course>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Course>>> GetAsync();

    Task<ActionResponse<Course>> AddAsync(CourseDTO entity);

    Task<ActionResponse<Course>> UpdateAsync(CourseDTO entity);

    Task<ActionResponse<Course>> DeleteAsync(int id);

    Task<IEnumerable<Course>> GetComboAsync(int id,bool indEsta);

    Task<ActionResponse<IEnumerable<Course>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}
