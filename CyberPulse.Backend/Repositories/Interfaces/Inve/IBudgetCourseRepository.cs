using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Inve;

public interface IBudgetCourseRepository
{
    Task<ActionResponse<BudgetCourse>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<BudgetCourse>>> GetAsync();

    Task<ActionResponse<BudgetCourse>> AddAsync(BudgetCourseDTO entity);

    Task<ActionResponse<BudgetCourse>> UpdateAsync(BudgetCourseDTO entity);

    Task<ActionResponse<BudgetCourse>> DeleteAsync(int id);

    Task<IEnumerable<BudgetCourse>> GetComboAsync();

    Task<ActionResponse<IEnumerable<BudgetCourse>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
    Task<ActionResponse<double>> GetBalanceAsync(int id);
}