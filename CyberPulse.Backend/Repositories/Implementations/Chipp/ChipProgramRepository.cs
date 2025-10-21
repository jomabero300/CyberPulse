using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO;
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
    public override async Task<ActionResponse<IEnumerable<ChipProgram>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.ChipPrograms.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Code.ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.Designation.ToLower().Contains(pagination.Filter.ToLower()));
        }

        var resul = await queryable
            .OrderBy(x => x.Code)
            .Paginate(pagination)
            .ToListAsync();

        return new ActionResponse<IEnumerable<ChipProgram>>
        {
            WasSuccess = true,
            Result = resul,
        };
    }
    public async Task<ActionResponse<ChipProgram>> GetAsync(string code)
    {
        var entity = await _context.ChipPrograms.AsNoTracking().Where(x => x.Code == code).FirstOrDefaultAsync();

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

    public async Task<IEnumerable<ChipProgram>> GetComboAsync(int id)
    {
        return id == 0 ? await _context.ChipPrograms
                                        .AsNoTracking()
                                        .OrderBy(x => x.Code)
                                        .ToListAsync() :
                        await _context.ChipPrograms
                                        .AsNoTracking()
                                        .Where(x => x.Id == id)
                                        .ToListAsync();
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.ChipPrograms.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => 
                                        x.Code.ToLower().Contains(pagination.Filter.ToLower()) || 
                                        x.Designation.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count,
        };
    }
}
