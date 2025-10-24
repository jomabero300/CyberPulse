using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.EntitiesDTO.Inve;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class ProductQuotationBodyUnitOfWork : GenericUnitOfWork<ProductQuotationBodyDTO>, IProductQuotationBodyUnitOfWork
{
    private readonly IProductQuotationBodyRepository _productQuotationBodyRepository;
    public ProductQuotationBodyUnitOfWork(IGenericRepository<ProductQuotationBodyDTO> repository, IProductQuotationBodyRepository productQuotationRepository) : base(repository)
    {
        _productQuotationBodyRepository = productQuotationRepository;
    }

    public async Task<IEnumerable<ProductQuotationBodyDTO>> GetComboAsync(int id)=>await _productQuotationBodyRepository.GetComboAsync(id);
}
