using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class CategoryUnitOfWork : GenericUnitOfWork<Category>, ICategoryUnitOfWork
{
    private ICategoryRepository _categoryRepository;
    public CategoryUnitOfWork(IGenericRepository<Category> repository, ICategoryRepository categoryRepository) : base(repository)
    {
        _categoryRepository = categoryRepository;
    }

    public override async Task<ActionResponse<Category>> GetAsync(int id)=>await _categoryRepository.GetAsync(id);
    public override async Task<ActionResponse<IEnumerable<Category>>> GetAsync(PaginationDTO pagination)=>await _categoryRepository.GetAsync(pagination);
    public override async Task<ActionResponse<Category>> DeleteAsync(int id)=>await _categoryRepository.DeleteAsync(id);

    public async Task<ActionResponse<Category>> AddAsync(CategoryDTO entity)=>await _categoryRepository.AddAsync(entity);
    public async Task<IEnumerable<Category>> GetComboAsync()=>await _categoryRepository.GetComboAsync();
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _categoryRepository.GetTotalRecordsAsync(pagination);
    public async Task<ActionResponse<Category>> UpdateAsync(CategoryDTO entity)=>await _categoryRepository.UpdateAsync(entity);
}
