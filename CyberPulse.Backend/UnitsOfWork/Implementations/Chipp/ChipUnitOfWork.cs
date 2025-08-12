using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Chipp;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.EntitiesDTO.Chipp.Report;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Chipp;

public class ChipUnitOfWork : GenericUnitOfWork<Chip>, IChipUnitOfWork
{
    private readonly IChipRepository _chipRepository;

    public ChipUnitOfWork(IGenericRepository<Chip> repository,IChipRepository chipRepository) : base(repository)
    {
        _chipRepository = chipRepository;
    }
    public override async Task<ActionResponse<IEnumerable<Chip>>> GetAsync()=>await _chipRepository.GetAsync();
    public override async Task<ActionResponse<Chip>> GetAsync(int id)=>await _chipRepository.GetAsync(id);

    public override async Task<ActionResponse<Chip>> DeleteAsync(int id)=>await _chipRepository.DeleteAsync(id);

    public override async Task<ActionResponse<IEnumerable<Chip>>> GetAsync(PaginationDTO pagination)=>await _chipRepository.GetAsync(pagination);

    public async Task<ActionResponse<Chip>> AddAsync(ChipDTO entity)=>await _chipRepository.AddAsync(entity);

    
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _chipRepository.GetTotalRecordsAsync(pagination); 

    public async Task<ActionResponse<Chip>> UpdateAsync(ChipDTO entity)=>await _chipRepository.UpdateAsync(entity);

    public async Task<ActionResponse<Chip>> UpdateAsync(ChipCoordinator entity) => await _chipRepository.UpdateAsync(entity);

    public async Task<ActionResponse<Chip>> GetAsync(ChipReportDTO entity)=>await _chipRepository.GetAsync(entity);

    public async Task<ActionResponse<IEnumerable<Chip>>> GetAsync(DateTime date)=>await _chipRepository.GetAsync(date);

    public async Task<ActionResponse<IEnumerable<Chip>>> GetAsync(ChipReport entity)=>await _chipRepository.GetAsync(entity);
}
