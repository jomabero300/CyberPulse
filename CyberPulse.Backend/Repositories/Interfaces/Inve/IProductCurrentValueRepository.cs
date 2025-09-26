using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Inve;

public interface IProductCurrentValueRepository
{
    Task<ActionResponse<ProductCurrentValue>> GetAsync(int id);
    Task<ActionResponse<ProductCurrentValue>> GetAsync(double id);
    Task<ActionResponse<IEnumerable<ProductCurrentValue>>> GetAsync();
    Task<ActionResponse<ProductCurrentValue>> AddAsync(ProductCurrentValueDTO entity);
    Task<ActionResponse<ProductCurrentValue>> UpdateAsync(ProductCurrentValueDTO entity);
    Task<ActionResponse<ProductCurrentValue>> DeleteAsync(int id);
    Task<IEnumerable<ProductCurrentValue>> GetComboAsync();
    Task<ActionResponse<IEnumerable<ProductCurrentValue>>> GetAsync(PaginationDTO pagination);
    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}