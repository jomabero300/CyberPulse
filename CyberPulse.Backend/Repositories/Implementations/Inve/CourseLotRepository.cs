using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class CourseLotRepository : GenericRepository<CourseLot>, ICourseLotRepository
{
    private readonly ApplicationDbContext _context;
    public CourseLotRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<CourseLot>> GetAsync(int id)
    {
        var entity = await _context.CourseLots
            .AsNoTracking()
            .Include(x => x.ProgramLot).ThenInclude(p=>p.Program)
            .Include(x => x.Course)
             .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new ActionResponse<CourseLot>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<CourseLot>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<IEnumerable<CourseLot>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.CourseLots
            .AsNoTracking()
            .Include(x => x.ProgramLot).ThenInclude(p=>p.Program)
            .Include(x => x.ProgramLot).ThenInclude(l=>l.Lot)
            .Include(x => x.Course)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => 
                                            x.Course!.Name.ToLower().Contains(pagination.Filter.ToLower())||
                                            x.ProgramLot!.Lot!.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
                                            x.ProgramLot!.Program!.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<CourseLot>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.ProgramLot!.Program!.Name).ThenBy(x => x.Course!.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }
    public override async Task<ActionResponse<CourseLot>> DeleteAsync(int id)
    {
        var entity = await _context.CourseLots.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<CourseLot>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<CourseLot>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<CourseLot>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }



    public async Task<ActionResponse<CourseLot>> AddAsync(CourseLotDTO entity)
    {
        var model = new CourseLot
        {
            Id = entity.Id,
            CourseId = entity.CourseId,
            ProgramLotId = entity.ProgramLotId,
        };

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<CourseLot>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<CourseLot>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<CourseLot>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }

    public Task<IEnumerable<CourseLot>> GetComboAsync(int id)
    {
        throw new NotImplementedException();
    }


    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.CourseLots
            .AsNoTracking()
            .Include(x=>x.ProgramLot).ThenInclude(p=>p.Program)
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

    public async Task<ActionResponse<CourseLot>> UpdateAsync(CourseLotDTO entity)
    {
        var model = await _context.CourseLots.FindAsync(entity.Id);

        if (model == null)
        {
            return new ActionResponse<CourseLot>
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

            return new ActionResponse<CourseLot>
            {
                WasSuccess = true,
                Result = model,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<CourseLot>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<CourseLot>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }

}
