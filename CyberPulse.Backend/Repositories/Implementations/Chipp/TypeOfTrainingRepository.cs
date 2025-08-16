using CyberPulse.Backend.Data;
using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Shared.Entities.Chipp;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Chipp;

public class TypeOfTrainingRepository : GenericRepository<TypeOfTraining>, ITypeOfTrainingRepository
{
    private readonly ApplicationDbContext _context;

    public TypeOfTrainingRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TypeOfTraining>> GetComboAsync()
    {
        return await _context.TypeOfTraining.AsNoTracking().OrderBy(x => x.Name).ToListAsync();
    }
}
