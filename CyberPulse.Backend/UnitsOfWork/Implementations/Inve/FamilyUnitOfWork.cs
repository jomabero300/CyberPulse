using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class FamilyUnitOfWork : GenericUnitOfWork<Family>, IFamilyUnitOfWork
{
    private readonly IFamilyRepository _familyRepository;
    public FamilyUnitOfWork(IGenericRepository<Family> repository, IFamilyRepository familyRepository) : base(repository)
    {
        _familyRepository = familyRepository;
    }

    public override async Task<ActionResponse<Family>> GetAsync(int id)=>await _familyRepository.GetAsync(id);
    public override async Task<ActionResponse<IEnumerable<Family>>> GetAsync(PaginationDTO pagination)=>await _familyRepository.GetAsync(pagination);
    public override async Task<ActionResponse<Family>> DeleteAsync(int id)=>await _familyRepository.DeleteAsync(id);


    public async Task<ActionResponse<Family>> AddAsync(FamilyDTO entity)=> await _familyRepository.AddAsync(entity);
    public async Task<IEnumerable<Family>> GetComboAsync(int id)=> await _familyRepository.GetComboAsync(id);
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _familyRepository.GetTotalRecordsAsync(pagination);
    public async Task<ActionResponse<Family>> UpdateAsync(FamilyDTO entity)=>await _familyRepository.UpdateAsync(entity);
}