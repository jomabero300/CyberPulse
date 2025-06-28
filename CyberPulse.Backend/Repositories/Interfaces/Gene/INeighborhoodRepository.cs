using CyberPulse.Shared.Entities.Gene;

namespace CyberPulse.Backend.Repositories.Interfaces.Gene;

public interface INeighborhoodRepository
{
    Task<IEnumerable<Neighborhood>> GetComboAsync(int id);
}
