using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;

public interface IInvProgramUnitOfWork
{
    Task<ActionResponse<InvProgram>> GetAsync(int id);
    //Task<ActionResponse<IEnumerable<InvProgram>>> GetAsync(int id, bool lb);

    Task<ActionResponse<IEnumerable<InvProgram>>> GetAsync();

    Task<ActionResponse<InvProgram>> AddAsync(InvProgramDTO entity);

    Task<ActionResponse<InvProgram>> UpdateAsync(InvProgramDTO entity);

    Task<ActionResponse<InvProgram>> DeleteAsync(int id);

    Task<IEnumerable<InvProgram>> GetComboAsync();

    Task<ActionResponse<IEnumerable<InvProgram>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}