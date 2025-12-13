using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class ProductCurrentValueUnitOfWork : GenericUnitOfWork<ProductCurrentValue>, IProductCurrentValueUnitOfWork
{
    private readonly IProductCurrentValueRepository _productCurrentValueRepository;
    public ProductCurrentValueUnitOfWork(IGenericRepository<ProductCurrentValue> repository, IProductCurrentValueRepository productCurrentValueRepository) : base(repository)
    {
        _productCurrentValueRepository = productCurrentValueRepository;
    }

    public override async Task<ActionResponse<ProductCurrentValue>> GetAsync(int id)=>await _productCurrentValueRepository.GetAsync(id);
    public override async Task<ActionResponse<IEnumerable<ProductCurrentValue>>> GetAsync(PaginationDTO pagination)=>await _productCurrentValueRepository.GetAsync(pagination);
    public override async Task<ActionResponse<ProductCurrentValue>> DeleteAsync(int id)=>await _productCurrentValueRepository.DeleteAsync(id);

    public async Task<ActionResponse<ProductCurrentValue>> AddAsync(ProductCurrentValueDTO entity)=>await _productCurrentValueRepository.AddAsync(entity);
    public async Task<IEnumerable<ProductCurrentValue>> GetComboAsync()=>await _productCurrentValueRepository.GetComboAsync();
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _productCurrentValueRepository.GetTotalRecordsAsync(pagination);
    public async Task<ActionResponse<ProductCurrentValue>> GetAsync(double id) => await _productCurrentValueRepository.GetAsync(id);
    public async Task<ActionResponse<IEnumerable<ProductCurrentValue>>> GetAsync(string Filter) => await _productCurrentValueRepository.GetAsync(Filter);
    public async Task<ActionResponse<ProductCurrentValue>> UpdateAsync(ProductCurrentValueDTO entity)=>await _productCurrentValueRepository.UpdateAsync(entity);

}
