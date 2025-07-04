using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Chipp;

public interface IChipProgramRepository
{
    Task<ActionResponse<ChipProgram>> GetAsync(string code);

    Task<IEnumerable<ChipProgram>> GetComboAsync(int id);

}
