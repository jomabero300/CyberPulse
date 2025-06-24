using CyberPulse.Shared.Entities.Chipp;

namespace CyberPulse.Backend.Repositories.Interfaces.Chipp;

public interface IChipProgramRepository
{
    Task<IEnumerable<ChipProgram>> GetComboAsync();

}
