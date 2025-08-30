using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;

public interface IBudgetTypeUnitOfWork
{
    Task<ActionResponse<BudgetType>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<BudgetType>>> GetAsync();

    Task<ActionResponse<BudgetType>> AddAsync(BudgetTypeDTO entity);

    Task<ActionResponse<BudgetType>> UpdateAsync(BudgetTypeDTO entity);

    Task<ActionResponse<BudgetType>> DeleteAsync(int id);

    Task<IEnumerable<BudgetType>> GetComboAsync();

    Task<ActionResponse<IEnumerable<BudgetType>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}