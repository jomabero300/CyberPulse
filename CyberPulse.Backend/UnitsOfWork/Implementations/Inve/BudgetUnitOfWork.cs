using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class BudgetUnitOfWork : GenericUnitOfWork<Budget>, IBudgetUnitOfWork
{
    private readonly IBudgetRepository _budgetRepository;
    public BudgetUnitOfWork(IGenericRepository<Budget> repository, IBudgetRepository budgetRepository) : base(repository)
    {
        _budgetRepository = budgetRepository;
    }

    public override async Task<ActionResponse<IEnumerable<Budget>>> GetAsync()=>await _budgetRepository.GetAsync();
    public override async Task<ActionResponse<Budget>> GetAsync(int id)=>await _budgetRepository.GetAsync(id);
    public override async Task<ActionResponse<IEnumerable<Budget>>> GetAsync(PaginationDTO pagination)=>await _budgetRepository.GetAsync(pagination);
    public override async Task<ActionResponse<Budget>> DeleteAsync(int id)=>await _budgetRepository.DeleteAsync(id);



    public async Task<ActionResponse<IEnumerable<Budget>>> GetAsync(string id) => await _budgetRepository.GetAsync(id);
    public async Task<ActionResponse<Budget>> AddAsync(BudgetDTO country)=>await _budgetRepository.AddAsync(country);

    public async Task<IEnumerable<Budget>> GetComboAsync()=>await _budgetRepository.GetComboAsync();

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _budgetRepository.GetTotalRecordsAsync(pagination);
    public async Task<ActionResponse<IEnumerable<Budget>>> GetAsync(string Filter, bool Statu) => await _budgetRepository.GetAsync(Filter, Statu);

    public async Task<ActionResponse<Budget>> UpdateAsync(BudgetDTO country)=>await _budgetRepository.UpdateAsync(country);

}
