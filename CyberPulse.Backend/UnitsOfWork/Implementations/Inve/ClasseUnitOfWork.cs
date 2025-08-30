using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class ClasseUnitOfWork : GenericUnitOfWork<Classe>, IClasseUnitOfWork
{
    private readonly IClasseRepository _classeRepository;
    public ClasseUnitOfWork(IGenericRepository<Classe> repository, IClasseRepository classeRepository) : base(repository)
    {
        _classeRepository = classeRepository;
    }

    public override async Task<ActionResponse<Classe>> GetAsync(int id)=>await _classeRepository.GetAsync(id);
    public override async Task<ActionResponse<IEnumerable<Classe>>> GetAsync(PaginationDTO pagination)=>await _classeRepository.GetAsync(pagination);
    public override async Task<ActionResponse<Classe>> DeleteAsync(int id)=>await _classeRepository.DeleteAsync(id);


    public async Task<ActionResponse<Classe>> AddAsync(ClasseDTO entity)=>await _classeRepository.AddAsync(entity);
    public async Task<IEnumerable<Classe>> GetComboAsync(int id)=>await _classeRepository.GetComboAsync(id);
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _classeRepository.GetTotalRecordsAsync(pagination);
    public async Task<ActionResponse<Classe>> UpdateAsync(ClasseDTO entity)=>await _classeRepository.UpdateAsync(entity);
}
