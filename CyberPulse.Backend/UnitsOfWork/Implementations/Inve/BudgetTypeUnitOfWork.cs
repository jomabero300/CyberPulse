using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class BudgetTypeUnitOfWork : GenericUnitOfWork<BudgetType>, IBudgetTypeUnitOfWork
{
    private readonly IBudgetTypeRepository _budgetTypeRepository;
    public BudgetTypeUnitOfWork(IGenericRepository<BudgetType> repository, IBudgetTypeRepository budgetTypeRepository) : base(repository)
    {
        _budgetTypeRepository = budgetTypeRepository;
    }


    public override async Task<ActionResponse<BudgetType>> GetAsync(int id)=>await _budgetTypeRepository.GetAsync(id);
    public override async Task<ActionResponse<IEnumerable<BudgetType>>> GetAsync(PaginationDTO pagination)=>await _budgetTypeRepository.GetAsync(pagination);
    public override async Task<ActionResponse<BudgetType>> DeleteAsync(int id)=>await _budgetTypeRepository.DeleteAsync(id);


    public async Task<ActionResponse<BudgetType>> AddAsync(BudgetTypeDTO entity)=>await _budgetTypeRepository.AddAsync(entity);
    public async Task<IEnumerable<BudgetType>> GetComboAsync()=>await _budgetTypeRepository.GetComboAsync();
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _budgetTypeRepository.GetTotalRecordsAsync(pagination);
    public async Task<ActionResponse<BudgetType>> UpdateAsync(BudgetTypeDTO entity)=>await _budgetTypeRepository.UpdateAsync(entity);
}
