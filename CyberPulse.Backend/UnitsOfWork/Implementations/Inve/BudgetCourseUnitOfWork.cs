using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class BudgetCourseUnitOfWork : GenericUnitOfWork<BudgetCourse>, IBudgetCourseUnitOfWork
{
    private readonly IBudgetCourseRepository _budgetCourseRepository;
    public BudgetCourseUnitOfWork(IGenericRepository<BudgetCourse> repository, IBudgetCourseRepository budgetCourseRepository) : base(repository)
    {
        _budgetCourseRepository = budgetCourseRepository;
    }
    public override async Task<ActionResponse<BudgetCourse>> GetAsync(int id)=>await _budgetCourseRepository.GetAsync(id);
    public override async Task<ActionResponse<IEnumerable<BudgetCourse>>> GetAsync(PaginationDTO pagination)=>await _budgetCourseRepository.GetAsync(pagination);
    public override async Task<ActionResponse<BudgetCourse>> DeleteAsync(int id)=>await _budgetCourseRepository.DeleteAsync(id);


    public async Task<ActionResponse<IEnumerable<BudgetCourse>>> GetAsync(string id) => await _budgetCourseRepository.GetAsync(id);
    public async Task<ActionResponse<BudgetCourse>> AddAsync(BudgetCourseDTO entity)=>await _budgetCourseRepository.AddAsync(entity);
    public async Task<IEnumerable<BudgetCourse>> GetComboAsync()=>await _budgetCourseRepository.GetComboAsync();
    public Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>_budgetCourseRepository.GetTotalRecordsAsync(pagination);
    public async Task<ActionResponse<BudgetCourse>> UpdateAsync(BudgetCourseDTO entity)=>await _budgetCourseRepository.UpdateAsync(entity);

    public async Task<ActionResponse<double>> GetBalanceAsync(int id)=>await _budgetCourseRepository.GetBalanceAsync(id);

    public async Task<ActionResponse<IEnumerable<BudgetCourse>>> GetAsync(string Filter, bool estado) => await _budgetCourseRepository.GetAsync(Filter, estado);
}
