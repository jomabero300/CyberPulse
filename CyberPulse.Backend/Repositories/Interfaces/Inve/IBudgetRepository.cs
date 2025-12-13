using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Inve;

public interface IBudgetRepository
{
    Task<ActionResponse<Budget>> GetAsync(int id);
    Task<ActionResponse<IEnumerable<Budget>>> GetAsync();
    Task<ActionResponse<IEnumerable<Budget>>> GetAsync(string id);
    Task<ActionResponse<IEnumerable<Budget>>> GetAsync(string Filter,bool Statu);
    Task<ActionResponse<Budget>> AddAsync(BudgetDTO country);
    Task<ActionResponse<Budget>> UpdateAsync(BudgetDTO country);
    Task<ActionResponse<Budget>> DeleteAsync(int id);
    Task<IEnumerable<Budget>> GetComboAsync();
    Task<ActionResponse<IEnumerable<Budget>>> GetAsync(PaginationDTO pagination);
    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}