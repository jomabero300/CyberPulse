using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Chipp;
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

    public async Task<ActionResponse<TrainingProgram>> AddAsync(TrainingProgramDTO entity)
    {
        var trainingProgram = new TrainingProgram
        {
            Id = entity.Id,
            Name = entity.Name,            
        };

        _context.Add(trainingProgram);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<TrainingProgram>
            {
                WasSuccess = true,
                Result = trainingProgram
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<TrainingProgram>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<TrainingProgram>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }

    public override async Task<ActionResponse<IEnumerable<TrainingProgram>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.TrainingPrograms.AsNoTracking().AsQueryable();

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
        var queryable = _context.TrainingPrograms.AsNoTracking().AsQueryable();

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

    public async Task<ActionResponse<TrainingProgram>> UpdateAsync(TrainingProgramDTO entity)
    {
        var trainingPrograms = await _context.TrainingPrograms.FindAsync(entity.Id);

        if (trainingPrograms == null)
        {
            return new ActionResponse<TrainingProgram>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        trainingPrograms.Name = entity.Name;

        _context.Update(trainingPrograms);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<TrainingProgram>
            {
                WasSuccess = true,
                Result = trainingPrograms,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<TrainingProgram>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<TrainingProgram>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }
}
