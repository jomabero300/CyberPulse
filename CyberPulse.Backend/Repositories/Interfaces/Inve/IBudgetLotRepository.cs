using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Inve;

public interface IBudgetLotRepository
{
    Task<ActionResponse<BudgetLot>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<BudgetLot>>> GetAsync();
    Task<ActionResponse<BudgetLot>> AddAsync(BudgetLotDTO entity);

    Task<ActionResponse<BudgetLot>> UpdateAsync(BudgetLotDTO entity);

    Task<ActionResponse<BudgetLot>> DeleteAsync(int id);

    //Task<IEnumerable<BudgetLot>> GetComboAsync();
    Task<IEnumerable<BudgetLot>> GetComboAsync(int id);

    Task<ActionResponse<IEnumerable<BudgetLot>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
    //Task<ActionResponse<double>> GetBalanceAsync(int id);
}