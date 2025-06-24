using CyberPulse.Shared.Entities.Chipp;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Chipp;

public interface IChipProgramUnitOfWork
{
    Task<IEnumerable<ChipProgram>> GetComboAsync();
}
