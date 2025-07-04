using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Chipp;

public interface IChipProgramUnitOfWork
{
    Task<ActionResponse<ChipProgram>> GetAsync(string code);

    Task<IEnumerable<ChipProgram>> GetComboAsync();
}
