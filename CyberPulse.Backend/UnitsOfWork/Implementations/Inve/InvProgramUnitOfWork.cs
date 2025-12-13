using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class InvProgramUnitOfWork : GenericUnitOfWork<InvProgram>, IInvProgramUnitOfWork
{
    private readonly IInvProgramRepository _invProgramRepository;
    public InvProgramUnitOfWork(IGenericRepository<InvProgram> repository, IInvProgramRepository invProgramRepository) : base(repository)
    {
        _invProgramRepository = invProgramRepository;
    }

    public override async Task<ActionResponse<InvProgram>> GetAsync(int id)=>await _invProgramRepository.GetAsync(id);
    public override async Task<ActionResponse<IEnumerable<InvProgram>>> GetAsync(PaginationDTO pagination)=>await _invProgramRepository.GetAsync(pagination);
    public override async Task<ActionResponse<InvProgram>> DeleteAsync(int id)=>await _invProgramRepository.DeleteAsync(id);

    public async Task<ActionResponse<InvProgram>> AddAsync(InvProgramDTO entity)=>await _invProgramRepository.AddAsync(entity);
    public async Task<IEnumerable<InvProgram>> GetComboAsync()=>await _invProgramRepository.GetComboAsync();
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _invProgramRepository.GetTotalRecordsAsync(pagination);
    public async Task<ActionResponse<IEnumerable<InvProgram>>> GetAsync(string Filter) => await _invProgramRepository.GetAsync(Filter);
    public async Task<ActionResponse<InvProgram>> UpdateAsync(InvProgramDTO entity)=>await _invProgramRepository.UpdateAsync(entity);

    //public async Task<ActionResponse<IEnumerable<InvProgram>>> GetAsync(int id, bool lb)=>await _invProgramRepository.GetAsync(id, lb);
}
