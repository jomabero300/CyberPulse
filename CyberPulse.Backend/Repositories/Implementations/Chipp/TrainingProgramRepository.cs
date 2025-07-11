using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Chipp;

public class TrainingProgramRepository : GenericRepository<TrainingProgram>, ITrainingProgramRepository
{
    private readonly ApplicationDbContext _context;
    public TrainingProgramRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<IEnumerable<TrainingProgram>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.TrainingPrograms.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        var resul = await queryable
            .OrderBy(x => x.Name)
            .Paginate(pagination)
            .ToListAsync();

        return new ActionResponse<IEnumerable<TrainingProgram>>
        {
            WasSuccess = true,
            Result = resul,
        };

    }
    public async Task<IEnumerable<TrainingProgram>> GetComboAsync()
    {
        return await _context.TrainingPrograms.OrderBy(x => x.Name).ToListAsync();
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.TrainingPrograms.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count,
        };

    }
}
