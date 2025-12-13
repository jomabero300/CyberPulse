using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;

public interface ICategoryUnitOfWork
{
    Task<ActionResponse<Category>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Category>>> GetAsync();

    Task<ActionResponse<Category>> AddAsync(CategoryDTO entity);

    Task<ActionResponse<Category>> UpdateAsync(CategoryDTO entity);

    Task<ActionResponse<Category>> DeleteAsync(int id);

    Task<IEnumerable<Category>> GetComboAsync();

    Task<ActionResponse<IEnumerable<Category>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}