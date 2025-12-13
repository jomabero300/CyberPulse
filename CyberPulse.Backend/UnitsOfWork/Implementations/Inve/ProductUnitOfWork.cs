using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class ProductUnitOfWork : GenericUnitOfWork<Product>, IProductUnitOfWork
{
    private readonly IProductRepository _productRepository;
    public ProductUnitOfWork(IGenericRepository<Product> repository, IProductRepository productRepository) : base(repository)
    {
        _productRepository = productRepository;
    }

    public override async Task<ActionResponse<Product>> GetAsync(int id)=>await _productRepository.GetAsync(id);
    public override async Task<ActionResponse<IEnumerable<Product>>> GetAsync(PaginationDTO pagination)=>await _productRepository.GetAsync(pagination);
    public override async Task<ActionResponse<Product>> DeleteAsync(int id)=>await _productRepository.DeleteAsync(id);


    public async Task<ActionResponse<Product>> AddAsync(ProductDTO entity)=>await _productRepository.AddAsync(entity);
    public async Task<IEnumerable<Product>> GetComboAsync()=>await _productRepository.GetComboAsync();
    public async Task<IEnumerable<Product>> GetComboAsync(int claseId, int lotId)=>await _productRepository.GetComboAsync(claseId, lotId);
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _productRepository.GetTotalRecordsAsync(pagination);
    public async Task<ActionResponse<IEnumerable<Product>>> GetAsync(string Filter) => await _productRepository.GetAsync(Filter);
    public async Task<ActionResponse<Product>> UpdateAsync(ProductDTO entity)=>await _productRepository.UpdateAsync(entity);

}
