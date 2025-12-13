using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;

public interface IProductQuotationUnitOfWork
{
    Task<ActionResponse<IEnumerable<ProductQuotation>>> GetAsync();
    Task<ActionResponse<ProductQuotation>> GetAsync(int id);
    Task<ActionResponse<bool>> GetAsync(int id, bool lb);
    Task<ActionResponse<ProductQuotation>> AddAsync(ProductQuotationDTO entity);
    Task<ActionResponse<List<ProductQuotation>>> AddAsync(ProductQuotationHeadDTO entity);
    Task<ActionResponse<ProductQuotation>> UpdateAsync(ProductQuotationDTO entity);
    Task<ActionResponse<ProductQuotation>> UpdateAsync(ProductQuotationPurcDTO entity);
    Task<ActionResponse<ProductQuotation>> UpdateAsync(ProductQuotationHeadDTO entity);
    Task<ActionResponse<ProductQuotation>> UpdateAsync(int id, int esta); 
    Task<ActionResponse<ProductQuotation>> UpdateAsync(int id);
    Task<ActionResponse<ProductQuotation>> DeleteAsync(int id);

    //Task<IEnumerable<ProductQuotation>> GetComboAsync();
    Task<ActionResponse<IEnumerable<ProductQuotation>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}