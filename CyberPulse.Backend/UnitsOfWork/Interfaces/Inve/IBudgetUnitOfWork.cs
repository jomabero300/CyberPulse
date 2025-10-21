using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;

public interface IBudgetUnitOfWork
{
    Task<ActionResponse<Budget>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Budget>>> GetAsync();

    Task<ActionResponse<Budget>> AddAsync(BudgetDTO country);

    Task<ActionResponse<Budget>> UpdateAsync(BudgetDTO country);

    Task<ActionResponse<Budget>> DeleteAsync(int id);

    Task<IEnumerable<Budget>> GetComboAsync();

    Task<ActionResponse<IEnumerable<Budget>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}