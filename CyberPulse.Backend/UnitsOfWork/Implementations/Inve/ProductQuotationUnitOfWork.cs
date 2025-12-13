using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class ProductQuotationUnitOfWork : GenericUnitOfWork<ProductQuotation>, IProductQuotationUnitOfWork
{
    private readonly IProductQuotationRepository _productQuotationRepository;
    public ProductQuotationUnitOfWork(IGenericRepository<ProductQuotation> repository, IProductQuotationRepository productQuotationRepository) : base(repository)
    {
        _productQuotationRepository = productQuotationRepository;
    }

    public override async Task<ActionResponse<ProductQuotation>> GetAsync(int id)=>await _productQuotationRepository.GetAsync(id);
    public override async Task<ActionResponse<IEnumerable<ProductQuotation>>> GetAsync(PaginationDTO pagination)=>await _productQuotationRepository.GetAsync(pagination);
    public override async Task<ActionResponse<ProductQuotation>> DeleteAsync(int id)=>await _productQuotationRepository.DeleteAsync(id);

    public async Task<ActionResponse<bool>> GetAsync(int id, bool lb) => await _productQuotationRepository.GetAsync(id, lb);
    public async Task<ActionResponse<ProductQuotation>> AddAsync(ProductQuotationDTO entity)=>await _productQuotationRepository.AddAsync(entity);
    public async Task<ActionResponse<List<ProductQuotation>>> AddAsync(ProductQuotationHeadDTO entity)=>await _productQuotationRepository.AddAsync(entity);
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _productQuotationRepository.GetTotalRecordsAsync(pagination);
    public async Task<ActionResponse<ProductQuotation>> UpdateAsync(ProductQuotationDTO entity)=>await _productQuotationRepository.UpdateAsync(entity);
    public async Task<ActionResponse<ProductQuotation>> UpdateAsync(ProductQuotationPurcDTO entity)=>await _productQuotationRepository.UpdateAsync(entity);
    public async Task<ActionResponse<ProductQuotation>> UpdateAsync(int id, int esta) => await _productQuotationRepository.UpdateAsync(id, esta);
    public async Task<ActionResponse<ProductQuotation>> UpdateAsync(int id) => await _productQuotationRepository.UpdateAsync(id);

    public async Task<ActionResponse<ProductQuotation>> UpdateAsync(ProductQuotationHeadDTO entity) => await _productQuotationRepository.UpdateAsync(entity);
}