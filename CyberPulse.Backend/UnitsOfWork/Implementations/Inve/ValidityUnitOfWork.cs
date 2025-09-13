using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class ValidityUnitOfWork : GenericUnitOfWork<Validity>, IValidityUnitOfWork
{
    private readonly IValidityRepository _validityRepository;
    public ValidityUnitOfWork(IGenericRepository<Validity> repository, IValidityRepository validityRepository) : base(repository)
    {
        _validityRepository = validityRepository;
    }

    public override async Task<ActionResponse<Validity>> GetAsync(int id)=>await _validityRepository.GetAsync(id);
    public override async Task<ActionResponse<IEnumerable<Validity>>> GetAsync(PaginationDTO pagination)=>await _validityRepository.GetAsync(pagination);
    public override async Task<ActionResponse<Validity>> DeleteAsync(int id)=>await _validityRepository.DeleteAsync(id);


    public async Task<ActionResponse<Validity>> AddAsync(ValidityDTO entity)=>await _validityRepository.AddAsync(entity);

    public async Task<IEnumerable<Validity>> GetComboAsync()=>await _validityRepository.GetComboAsync();

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _validityRepository.GetTotalRecordsAsync(pagination);

    public async Task<ActionResponse<Validity>> UpdateAsync(ValidityDTO entity)=> await _validityRepository.UpdateAsync(entity);

    public async Task<ActionResponse<int>> GetNewValidityAsync()=>await _validityRepository.GetNewValidityAsync();
}
