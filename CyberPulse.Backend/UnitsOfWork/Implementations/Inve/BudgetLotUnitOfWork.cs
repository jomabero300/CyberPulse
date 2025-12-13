using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class BudgetLotUnitOfWork : GenericUnitOfWork<BudgetLot>, IBudgetLotUnitOfWork
{
    private readonly IBudgetLotRepository _budgetLotRepository;
    public BudgetLotUnitOfWork(IGenericRepository<BudgetLot> repository, IBudgetLotRepository budgetLotRepository) : base(repository)
    {
        _budgetLotRepository = budgetLotRepository;
    }

    public override async Task<ActionResponse<BudgetLot>> GetAsync(int id)=>await _budgetLotRepository.GetAsync(id);
    public override async Task<ActionResponse<IEnumerable<BudgetLot>>> GetAsync(PaginationDTO pagination)=>await _budgetLotRepository.GetAsync(pagination);
    public override async Task<ActionResponse<BudgetLot>> DeleteAsync(int id)=>await _budgetLotRepository.DeleteAsync(id);


    public async Task<ActionResponse<BudgetLot>> AddAsync(BudgetLotDTO entity) => await _budgetLotRepository.AddAsync(entity);
    public async Task<IEnumerable<BudgetLot>> GetComboAsync(int id) => await _budgetLotRepository.GetComboAsync(id);
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _budgetLotRepository.GetTotalRecordsAsync(pagination);
    public async Task<ActionResponse<double>> GetBalanceAsync(int id) => await _budgetLotRepository.GetBalanceAsync(id);
    public async Task<ActionResponse<IEnumerable<BudgetLot>>> GetAsync(string Filter) => await _budgetLotRepository.GetAsync(Filter);
    public async Task<ActionResponse<BudgetLot>> UpdateAsync(BudgetLotDTO entity)=>await _budgetLotRepository.UpdateAsync(entity);

}
