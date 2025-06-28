using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Gene;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse.Shared.Entities.Gene;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Gene;

public class NeighborhoodUnitOfWork : GenericUnitOfWork<Neighborhood>, INeighborhoodUnitOfWork
{
    private readonly INeighborhoodRepository _neighborhood;
    public NeighborhoodUnitOfWork(IGenericRepository<Neighborhood> repository, INeighborhoodRepository neighborhood) : base(repository)
    {
        _neighborhood = neighborhood;
    }

    public async Task<IEnumerable<Neighborhood>> GetComboAsync(int id)=>await _neighborhood.GetComboAsync(id);
}
