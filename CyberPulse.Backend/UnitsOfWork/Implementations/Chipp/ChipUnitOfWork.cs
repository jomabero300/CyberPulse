using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Chipp;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Chipp;

public class ChipUnitOfWork : GenericUnitOfWork<Chip>, IChipUnitOfWork
{
    private readonly IChipRepository _chipRepository;

    public ChipUnitOfWork(IGenericRepository<Chip> repository,IChipRepository chipRepository) : base(repository)
    {
        _chipRepository = chipRepository;
    }

    public async Task<ActionResponse<Chip>> AddAsync(ChipDTO entity)=>await _chipRepository.AddAsync(entity);

    public async Task<IEnumerable<Chip>> GetComboAsync(int id)=>await _chipRepository.GetComboAsync(id);

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _chipRepository.GetTotalRecordsAsync(pagination); 

    public async Task<ActionResponse<Chip>> UpdateAsync(ChipDTO entity)=>await _chipRepository.UpdateAsync(entity);
}
