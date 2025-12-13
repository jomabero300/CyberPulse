using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class ProgramLotUnitOfWork : GenericUnitOfWork<ProgramLot>, IProgramLotUnitOfWork
{
    private readonly IProgramLotRepository _programLotRepository;
    public ProgramLotUnitOfWork(IGenericRepository<ProgramLot> repository, IProgramLotRepository programLotRepository) : base(repository)
    {
        _programLotRepository = programLotRepository;
    }

    public override async Task<ActionResponse<ProgramLot>> GetAsync(int id)=>await _programLotRepository.GetAsync(id);
    public override async Task<ActionResponse<IEnumerable<ProgramLot>>> GetAsync(PaginationDTO pagination)=>await _programLotRepository.GetAsync(pagination);
    public override async Task<ActionResponse<ProgramLot>> DeleteAsync(int id)=>await _programLotRepository.DeleteAsync(id);


    public async Task<ActionResponse<ProgramLot>> AddAsync(ProgramLotDTO entity)=>await _programLotRepository.AddAsync(entity);
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _programLotRepository.GetTotalRecordsAsync(pagination);
    public async Task<ActionResponse<IEnumerable<ProgramLot>>> GetAsync(string Filter) => await _programLotRepository.GetAsync(Filter);
    public async Task<ActionResponse<ProgramLot>> UpdateAsync(ProgramLotDTO entity)=>await _programLotRepository.UpdateAsync(entity);

    public async Task<IEnumerable<ProgramLot>> GetComboAsync(int id)=>await _programLotRepository.GetComboAsync(id);

}
