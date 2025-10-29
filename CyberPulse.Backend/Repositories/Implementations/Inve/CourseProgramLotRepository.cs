using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class CourseProgramLotRepository : GenericRepository<CourseProgramLot>, ICourseProgramLotRepository
{
    private readonly ApplicationDbContext _context;
    public CourseProgramLotRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<CourseProgramLot>> GetAsync(int id)
    {
        var entity = await _context.CourseProgramLots
            .AsNoTracking()
            .Include(x => x.ProgramLot).ThenInclude(p=>p!.Program)
            .Include(x => x.Course)
             .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new ActionResponse<CourseProgramLot>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<CourseProgramLot>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<IEnumerable<CourseProgramLot>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.CourseProgramLots
            .AsNoTracking()
            .Include(x => x.ProgramLot).ThenInclude(p=>p!.Program)
            .Include(x => x.ProgramLot).ThenInclude(l=>l!.Lot)
            .Include(x => x.Course)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => 
                                            x.Course!.Name.ToLower().Contains(pagination.Filter.ToLower())||
                                            x.ProgramLot!.Lot!.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
                                            x.ProgramLot!.Program!.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<CourseProgramLot>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.ProgramLot!.Program!.Name).ThenBy(x => x.Course!.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }
    public override async Task<ActionResponse<CourseProgramLot>> DeleteAsync(int id)
    {
        var entity = await _context.CourseProgramLots.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<CourseProgramLot>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<CourseProgramLot>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<CourseProgramLot>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }



    public async Task<ActionResponse<CourseProgramLot>> AddAsync(CourseProgramLotDTO entity)
    {
        var model = new CourseProgramLot
        {
            Id = entity.Id,
            CourseId = entity.CourseId,
            ProgramLotId = entity.ProgramLotId,
        };

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<CourseProgramLot>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<CourseProgramLot>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<CourseProgramLot>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }
    public async Task<IEnumerable<CourseProgramLot>> GetComboAsync(int id)
    {
        return await _context.CourseProgramLots
                                 .Include(x => x.Course)
                                 .Include(x => x.ProgramLot).ThenInclude(x=>x!.Lot)
                                 .Where(x=>x.ProgramLotId==id)
                                 .OrderBy(x=>x.Course!.Name)
                                 .ToListAsync();
    }
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.CourseProgramLots
            .AsNoTracking()
            .Include(x=>x.ProgramLot).ThenInclude(p=>p!.Program)
            .Include(x=>x.Course)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Course!.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.ProgramLot!.Program!.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }
    public async Task<ActionResponse<CourseProgramLot>> UpdateAsync(CourseProgramLotDTO entity)
    {
        var model = await _context.CourseProgramLots.FindAsync(entity.Id);

        if (model == null)
        {
            return new ActionResponse<CourseProgramLot>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        model.ProgramLotId = entity.ProgramLotId;
        model.CourseId = entity.CourseId;

        _context.Update(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<CourseProgramLot>
            {
                WasSuccess = true,
                Result = model,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<CourseProgramLot>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<CourseProgramLot>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }

}
