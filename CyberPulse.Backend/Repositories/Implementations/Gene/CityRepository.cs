using CyberPulse.Backend.Data;
using CyberPulse.Backend.Repositories.Interfaces.Gene;
using CyberPulse.Shared.Entities.Gene;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Gene;

public class CityRepository : GenericRepository<City>, ICityRepository
{
    private readonly ApplicationDbContext _context;

    public CityRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<City>> GetComboAsync(int id)
    {
        return await _context.Cities.AsNoTracking().Where(x => x.StateId == id).OrderBy(x=>x.Name).ToListAsync();
    }
}
