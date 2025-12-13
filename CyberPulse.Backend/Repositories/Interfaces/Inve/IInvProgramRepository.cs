using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Inve;

public interface IInvProgramRepository
{
    Task<ActionResponse<InvProgram>> GetAsync(int id);
    Task<ActionResponse<IEnumerable<InvProgram>>> GetAsync(string Filter);

    Task<ActionResponse<IEnumerable<InvProgram>>> GetAsync();

    Task<ActionResponse<InvProgram>> AddAsync(InvProgramDTO entity);

    Task<ActionResponse<InvProgram>> UpdateAsync(InvProgramDTO entity);

    Task<ActionResponse<InvProgram>> DeleteAsync(int id);

    Task<IEnumerable<InvProgram>> GetComboAsync();

    Task<ActionResponse<IEnumerable<InvProgram>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);

}