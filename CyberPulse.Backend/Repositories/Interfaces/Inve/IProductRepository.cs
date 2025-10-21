using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Inve;

public interface IProductRepository
{
    Task<ActionResponse<Product>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Product>>> GetAsync();

    Task<ActionResponse<Product>> AddAsync(ProductDTO entity);

    Task<ActionResponse<Product>> UpdateAsync(ProductDTO entity);

    Task<ActionResponse<Product>> DeleteAsync(int id);

    Task<IEnumerable<Product>> GetComboAsync(int claseId,int lotId);
    Task<IEnumerable<Product>> GetComboAsync();
    Task<ActionResponse<IEnumerable<Product>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}
