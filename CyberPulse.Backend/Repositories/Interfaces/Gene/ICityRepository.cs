using CyberPulse.Shared.Entities.Gene;

namespace CyberPulse.Backend.Repositories.Interfaces.Gene;

public interface ICityRepository
{
    Task<IEnumerable<City>> GetComboAsync(int id);
}
