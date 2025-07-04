using CyberPulse.Backend.Data;
using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.Enums;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Chipp;

public class ChipProgramRepository : GenericRepository<ChipProgram>, IChipProgramRepository
{
    private readonly ApplicationDbContext _context;

    public ChipProgramRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ActionResponse<ChipProgram>> GetAsync(string code)
    {
        var entity = await _context.ChipPrograms.Where(x => x.Code == code).FirstOrDefaultAsync();

        if (entity == null)
        {
            return new ActionResponse<ChipProgram>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<ChipProgram>
        {
            WasSuccess = true,
            Result = entity
        };
    }

    public async Task<IEnumerable<ChipProgram>> GetComboAsync()
    {
        return await _context.ChipPrograms
            .OrderBy(x => x.Code).ToListAsync();
    }
}
