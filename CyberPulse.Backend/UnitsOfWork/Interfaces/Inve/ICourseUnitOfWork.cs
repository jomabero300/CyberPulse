using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;

public interface ICourseUnitOfWork
{
    Task<ActionResponse<Course>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Course>>> GetAsync();
    Task<ActionResponse<IEnumerable<Course>>> GetAsync(string Filter);
    Task<ActionResponse<Course>> AddAsync(CourseDTO entity);

    Task<ActionResponse<Course>> UpdateAsync(CourseDTO entity);

    Task<ActionResponse<Course>> DeleteAsync(int id);

    Task<IEnumerable<Course>> GetComboAsync(int id,bool indEsta);

    Task<ActionResponse<IEnumerable<Course>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}