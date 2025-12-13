using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class BudgetProgramUnitOfWork : GenericUnitOfWork<BudgetProgram>, IBudgetProgramUnitOfWork
{
    private readonly IBudgetProgramRepository _budgetProgramRepository;
    public BudgetProgramUnitOfWork(IGenericRepository<BudgetProgram> repository, IBudgetProgramRepository budgetProgramRepository) : base(repository)
    {
        _budgetProgramRepository = budgetProgramRepository;
    }


    public override async Task<ActionResponse<BudgetProgram>> GetAsync(int id)=>await _budgetProgramRepository.GetAsync(id);
    public override async Task<ActionResponse<IEnumerable<BudgetProgram>>> GetAsync(PaginationDTO pagination)=>await _budgetProgramRepository.GetAsync(pagination);
    public override async Task<ActionResponse<BudgetProgram>> DeleteAsync(int id)=>await _budgetProgramRepository.DeleteAsync(id);


    public async Task<ActionResponse<BudgetProgram>> AddAsync(BudgetProgramDTO entity)=>await _budgetProgramRepository.AddAsync(entity);
    //public async Task<IEnumerable<BudgetProgram>> GetComboAsync(int id)=>await _budgetProgramRepository.GetComboAsync(id);
    public async Task<IEnumerable<BudgetProgram>> GetComboAsync()=>await _budgetProgramRepository.GetComboAsync();
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _budgetProgramRepository.GetTotalRecordsAsync(pagination);
    public async Task<ActionResponse<IEnumerable<BudgetProgram>>> GetAsync(string Filter) => await _budgetProgramRepository.GetAsync(Filter);
    public async Task<ActionResponse<BudgetProgram>> UpdateAsync(BudgetProgramDTO entity)=>await _budgetProgramRepository.UpdateAsync(entity);

    public async Task<ActionResponse<double>> GetBalanceAsync(int id)=>await _budgetProgramRepository.GetBalanceAsync(id);

}
