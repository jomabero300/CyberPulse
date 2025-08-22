using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Gene;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Gene;

public class StatuUnitOfWork : GenericUnitOfWork<Statu>, IStatuUnitOfWork
{
    private readonly IStatuRepository _statuRepository;

    public StatuUnitOfWork(IGenericRepository<Statu> repository, IStatuRepository statuRepository) : base(repository)
    {
        _statuRepository = statuRepository;
    }

    public override async Task<ActionResponse<IEnumerable<Statu>>> GetAsync() => await _statuRepository.GetAsync();
    public async Task<ActionResponse<Statu>> GetAsync(string name, int nivel) => await _statuRepository.GetAsync(name, nivel);
    public override async Task<ActionResponse<IEnumerable<Statu>>> GetAsync(PaginationDTO pagination) => await _statuRepository.GetAsync(pagination);
    public override async Task<ActionResponse<Statu>> GetAsync(int id) => await _statuRepository.GetAsync(id);
    public override async Task<ActionResponse<Statu>> DeleteAsync(int id) => await _statuRepository.DeleteAsync(id);


    //public async Task<ActionResponse<Statu>> AddAsync(StatuDTO statuDTO)=>await _statuRepository.AddAsync(statuDTO);
    public async Task<IEnumerable<Statu>> GetComboAsync(int id) => await _statuRepository.GetComboAsync(id);
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _statuRepository.GetTotalRecordsAsync(pagination);

    //public async Task<ActionResponse<Statu>> UpdateAsync(StatuDTO statuDTO) => await _statuRepository.UpdateAsync(statuDTO);
}
