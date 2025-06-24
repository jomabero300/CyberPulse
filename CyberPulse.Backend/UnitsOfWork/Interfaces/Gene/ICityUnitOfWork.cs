using CyberPulse.Shared.Entities.Gene;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;

public interface ICityUnitOfWork
{
    Task<IEnumerable<City>> GetComboAsync(int id);
}
