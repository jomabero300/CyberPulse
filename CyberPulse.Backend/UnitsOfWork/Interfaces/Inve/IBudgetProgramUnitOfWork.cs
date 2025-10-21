using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;

public interface IBudgetProgramUnitOfWork
{
    Task<ActionResponse<BudgetProgram>> GetAsync(int id);
    Task<ActionResponse<IEnumerable<BudgetProgram>>> GetAsync();
    Task<ActionResponse<BudgetProgram>> AddAsync(BudgetProgramDTO entity);
    Task<ActionResponse<BudgetProgram>> UpdateAsync(BudgetProgramDTO entity);
    Task<ActionResponse<BudgetProgram>> DeleteAsync(int id);
    //Task<IEnumerable<BudgetProgram>> GetComboAsync(int id);
    Task<IEnumerable<BudgetProgram>> GetComboAsync();
    Task<ActionResponse<IEnumerable<BudgetProgram>>> GetAsync(PaginationDTO pagination);
    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
    Task<ActionResponse<double>> GetBalanceAsync(int id);
}