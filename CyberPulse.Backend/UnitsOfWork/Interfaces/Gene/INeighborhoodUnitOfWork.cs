using CyberPulse.Shared.Entities.Gene;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;

public interface INeighborhoodUnitOfWork
{
    Task<IEnumerable<Neighborhood>> GetComboAsync(int id);
}
