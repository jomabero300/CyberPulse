using CyberPulse.Backend.Data;
using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Shared.Entities.Chipp;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Chipp;

public class ChipProgramRepository : GenericRepository<ChipProgram>, IChipProgramRepository
{
    private readonly ApplicationDbContext _context;

    public ChipProgramRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ChipProgram>> GetComboAsync()
    {
        return await _context.ChipPrograms
            .OrderBy(x => x.Code).ToListAsync();
    }
}
